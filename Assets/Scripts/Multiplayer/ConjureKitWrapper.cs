using System;
using Auki.ConjureKit;
using Auki.ConjureKit.Manna;
using UnityEngine;

public class ConjureKitWrapper : MonoBehaviour
{
    [SerializeField] private Transform arCamera;
    [SerializeField] private Transform environment;

    private IConjureKit _conjureKit;
    private Manna _manna;

    public OrnamentSystem OrnamentSystem { get; private set; }
    
    private void Start()
    {
        _conjureKit = new ConjureKit(
            arCamera,
            "YOUR_APP_KEY",
            "YOUR_APP_SECRET");

        _conjureKit.OnJoined += session =>
        {
            OrnamentSystem = new OrnamentSystem(session);
            session.RegisterSystem(OrnamentSystem, () => Debug.Log("Ornament system registered successfully"));
        };

        _manna = new Manna(_conjureKit);
        _manna.GetOrCreateFrameFeederComponent().AttachMannaInstance(_manna);

        _manna.OnLighthouseTracked += OnQRCodeDetected;

        _conjureKit.Connect();
    }

    private void OnQRCodeDetected(Lighthouse lighthouse, Pose pose, bool isClose)
    {
        environment.transform.position = pose.position;
    }
}
