using System;
using Auki.ConjureKit;
using Auki.ConjureKit.Manna;
using Auki.Integration.ARFoundation.Manna;
using UnityEngine;

public class ConjureKitWrapper : MonoBehaviour
{
    [SerializeField] private Transform arCamera;

    public IConjureKit ConjureKit => _conjureKit;
    public event Action<string> OnDomainEntered;

    private IConjureKit _conjureKit;
    private Manna _manna;

    private string _currentDomainId;
    private bool _isNewDomain;

    private void Start()
    {
        _conjureKit = new ConjureKit(
            arCamera,
            "d69fb2b9-3e83-47c8-95a2-26a12796e2e1",
            "6764b692-e8d0-4a07-baff-c0804d4b4ece96254774-50a1-4818-b96b-0992cacb8a26");
        
        _manna = new Manna(_conjureKit);

#if UNITY_EDITOR
        arCamera.gameObject.AddComponent<FrameFeederEditor>().AttachMannaInstance(_manna);
#else
        _manna.GetOrCreateFrameFeederComponent().AttachMannaInstance(_manna);
#endif

        _manna.OnLighthouseTracked += OnQRCodeDetected;
        _manna.SetStaticLighthousePoseSelector(OnStaticLighthouseScanned);
        
        _conjureKit.Init(ConjureKitConfiguration.Get());
    }

    private void OnStaticLighthouseScanned(LighthousePose[] lighthouses, Action<LighthousePose> selectLighthouse)
    {
        if (lighthouses[0].domainId != _currentDomainId)
        {
            _isNewDomain = true;
            _currentDomainId = lighthouses[0].domainId;
            selectLighthouse?.Invoke(lighthouses[0]);
        }
    }

    private void OnQRCodeDetected(Lighthouse lighthouse, Pose pose, bool isClose)
    {
        if (isClose && _isNewDomain)
        {
            _isNewDomain = false;
            OnDomainEntered?.Invoke(_currentDomainId);
        }
    }
}
