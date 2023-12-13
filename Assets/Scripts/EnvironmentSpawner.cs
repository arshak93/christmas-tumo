using System;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform environment;

    private bool _isMoving;

    public void StartStopMoving()
    {
        _isMoving = !_isMoving;
    }

    private void Update()
    {
        if (_isMoving)
        {
            environment.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 8 + Vector3.down * 2;
        }
    }
}