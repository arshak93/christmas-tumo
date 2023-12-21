using Multiplayer;
using UnityEngine;

public class ResourcePrefabComponent : ComponentBase
{
    public string prefabAddress;
    
    private ResourcePrefabSpawner _resourcePrefabSpawner;
    
    private void Start()
    {
        _resourcePrefabSpawner = FindObjectOfType<ResourcePrefabSpawner>();
        if (_resourcePrefabSpawner == null)
        {
            Debug.LogError($"Object of type ResourcePrefabSpawner is required by component ResourcePrefabComponent on object {gameObject.name}");
            return;
        }
    }

    protected override void OnEntityReady(ConjureKitWrapper conjureKitWrapper)
    {
        if (this.EntityGameObject.IsOwn == true)
        {
            _resourcePrefabSpawner.RegisterOwnComponent(this);
        }
    }
}