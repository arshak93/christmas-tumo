using System;
using Auki.ConjureKit;
using Auki.ConjureKit.Manna;
using Auki.Integration.ARFoundation.Manna;
using UnityEngine;

namespace Posemesh
{
    public class ConjureKitWrapper : MonoBehaviour
    {
        [SerializeField] private Transform arCamera;

        public event Action<string> OnDomainEntered;

        private IConjureKit _conjureKit;
        private Manna _manna;

        private string _currentDomainId;
        private bool _isNewDomain;

        private void Start()
        {
            // Initialize ConjureKit and Manna modules
            _conjureKit = new ConjureKit(
                arCamera,
                "YOUR_APP_KEY",
                "YOUR_APP_SECRET");
        
            _manna = new Manna(_conjureKit);

            // Initialize the frame feeder component to let Manna detect QR codes from the camera feed and do the calibration
#if UNITY_EDITOR
            arCamera.gameObject.AddComponent<FrameFeederEditor>().AttachMannaInstance(_manna);
#else
        _manna.GetOrCreateFrameFeederComponent().AttachMannaInstance(_manna);
#endif
        
            _manna.SetStaticLighthousePoseSelector(OnStaticLighthouseScanned);
            _manna.OnLighthouseTracked += OnQRCodeDetected;
        
            // Initialize the ConjureKit without connecting to a session since we don't need realtime multiplayer
            // for this example and only need the calibration
            _conjureKit.Init(ConjureKitConfiguration.Get());
        }

        /// <summary>
        /// This callback is called when Manna detects a new QR code.
        /// A single QR code can be part of multiple domains
        /// but for this example we'll always select the first domain.
        /// </summary>
        private void OnStaticLighthouseScanned(LighthousePose[] lighthouses, Action<LighthousePose> selectLighthouse)
        {
            if (lighthouses[0].domainId != _currentDomainId)
            {
                _isNewDomain = true;
                _currentDomainId = lighthouses[0].domainId;
                selectLighthouse?.Invoke(lighthouses[0]);
            }
        }

        /// <summary>
        /// This callback will be invoked after we selected the domain.
        /// At this point Manna already loaded the pose information and did the calibration. 
        /// </summary>
        private void OnQRCodeDetected(Lighthouse lighthouse, Pose pose, bool isClose)
        {
            // Invoke the callback if it's from a new domain and
            // the QR code was close to the camera, meaning the we have good calibration
            if (isClose && _isNewDomain)
            {
                _isNewDomain = false;
                OnDomainEntered?.Invoke(_currentDomainId);
            }
        }
    }
}
