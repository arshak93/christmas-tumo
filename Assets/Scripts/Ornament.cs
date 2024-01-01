using System;
using UnityEngine;

public class Ornament : MonoBehaviour
{
    [SerializeField] private Renderer bodyRenderer;
    [SerializeField] private HingeJoint hingeJoint;
    [SerializeField] private Rigidbody hangPoint;
    [SerializeField] private Sprite screenshot;

    private Camera _mainCamera;
    private OrnamentPosition _position;

    public string text;
    public Sprite Screenshot => screenshot;
    public string Text => text;

    public OrnamentPosition CurrentPosition
    {
        get => _position;
        set
        {
            _position = value;
            // hingeJoint.anchor = _position.transform.position;
        }
    }

    public void SetMaterial(Material material)
    {
        bodyRenderer.sharedMaterial = material;
    }

    public void SetData(OrnamentData data)
    {
        bodyRenderer.sharedMaterial = Resources.Load<Material>(data.material);
    }

    public void Hang(OrnamentPosition ornamentPosition)
    {
        _position = ornamentPosition;

        if (_position != null)
        {
            // hingeJoint.anchor = _position.transform.position;
            // hangPoint.transform.position = _position.transform.position
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // if (_position == null)
        // {
        //     hangPoint.transform.position = _mainCamera.transform.position +
        //                                    _mainCamera.transform.forward * 1.8f;
        // }
    }
}