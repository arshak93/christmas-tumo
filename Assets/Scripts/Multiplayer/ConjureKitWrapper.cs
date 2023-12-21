using System;
using Auki.ConjureKit;
using Auki.ConjureKit.Manna;
using UnityEngine;

public class ConjureKitWrapper : MonoBehaviour
{
    [SerializeField] private Transform arCamera;
    [SerializeField] private Transform environment;

    public IConjureKit ConjureKit => _conjureKit;

    public Action<Session> OnJoined;

    private IConjureKit _conjureKit;
    private Manna _manna;

    public ResourcePrefabSystem ResourcePrefabSystem { get; private set; }
    public OrnamentSystem OrnamentSystem { get; private set; }
    
    private void Start()
    {
        _conjureKit = new ConjureKit(
            arCamera,
            "d69fb2b9-3e83-47c8-95a2-26a12796e2e1",
            "6764b692-e8d0-4a07-baff-c0804d4b4ece96254774-50a1-4818-b96b-0992cacb8a26");

        _conjureKit.OnJoined += session =>
        {
            ResourcePrefabSystem = new ResourcePrefabSystem(session);
            OrnamentSystem = new OrnamentSystem(session);
            session.RegisterSystem(ResourcePrefabSystem, () => Debug.Log("Resource Prefab system registered successfully"));
            session.RegisterSystem(OrnamentSystem, () => Debug.Log("Ornament system registered successfully"));
            
            OnJoined?.Invoke(session);
        };

        _manna = new Manna(_conjureKit);
        _manna.GetOrCreateFrameFeederComponent().AttachMannaInstance(_manna);

        _manna.OnLighthouseTracked += OnQRCodeDetected;
        
        _manna.SetStaticLighthousePoseSelector(OnStaticLighthouseScanned);

        _conjureKit.Connect();
    }

    private void OnStaticLighthouseScanned(LighthousePose[] lighthouses, Action<LighthousePose> SelectLighthouse)
    {
        
    }

    private void OnQRCodeDetected(Lighthouse lighthouse, Pose pose, bool isClose)
    {
        environment.gameObject.SetActive(true);
        environment.transform.position = pose.position;
    }
}
