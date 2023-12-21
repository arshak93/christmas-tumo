using System;
using System.Collections.Generic;
using Auki.ConjureKit;
using Multiplayer;
using UnityEngine;

public class ResourcePrefabSpawner : MonoBehaviour
{
    [SerializeField] private ConjureKitWrapper conjureKitWrapper;

    private Dictionary<uint, ResourcePrefabComponent> _resourcePrefabComponents =
        new Dictionary<uint, ResourcePrefabComponent>();

    private void Start()
    {
        conjureKitWrapper.OnJoined += OnJoined;
    }
    
    private void OnJoined(Session session)
    {
        conjureKitWrapper.ResourcePrefabSystem.OnComponentUpdated += OnResourcePrefabComponentUpdated;
    }

    public void RegisterOwnComponent(ResourcePrefabComponent component)
    {
        var entityId = component.GetComponent<EntityGameObject>().EntityId;
        conjureKitWrapper.ResourcePrefabSystem.SetComponent(entityId, new ResourcePrefabData(component.prefabAddress));
        _resourcePrefabComponents.Add(entityId, component);
    }

    private void OnResourcePrefabComponentUpdated(uint entityId, ResourcePrefabData data)
    {
        if (_resourcePrefabComponents.ContainsKey(entityId))
        {
            Destroy(_resourcePrefabComponents[entityId].gameObject);
        }
        
        var newComponent = Instantiate(Resources.Load<ResourcePrefabComponent>(data.prefabAddress));
        newComponent.prefabAddress = data.prefabAddress;
        _resourcePrefabComponents[entityId] = newComponent;
    }
}