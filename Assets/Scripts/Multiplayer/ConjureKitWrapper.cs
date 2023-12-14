using System;
using System.Collections;
using System.Collections.Generic;
using Auki.ConjureKit;
using Auki.ConjureKit.Manna;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ConjureKitWrapper : MonoBehaviour
{
    [SerializeField] private Transform arCamera;
    [SerializeField] private Transform environment;

    private IConjureKit _conjureKit;
    private Manna _manna;
    
    private void Start()
    {
        _conjureKit = new ConjureKit(
            arCamera,
            "d69fb2b9-3e83-47c8-95a2-26a12796e2e1",
            "6764b692-e8d0-4a07-baff-c0804d4b4ece96254774-50a1-4818-b96b-0992cacb8a26");

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
