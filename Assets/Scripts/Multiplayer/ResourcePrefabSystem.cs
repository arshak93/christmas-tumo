using System;
using System.Collections.Generic;
using System.Text;
using Auki.ConjureKit;
using Auki.ConjureKit.ECS;
using Multiplayer;
using UnityEngine;

public class ResourcePrefabSystem : SystemBase
{
    public const string COMPONENT_NAME = "resource_prefab";
    
    public event Action<uint, ResourcePrefabData> OnComponentUpdated;
    
    private readonly IDictionary<uint, ResourcePrefabData> _dataMap = new Dictionary<uint, ResourcePrefabData>();

    public ResourcePrefabSystem(Session session) : base(session)
    {
    }
    
    public override string[] GetComponentTypeNames()
    {
        return new[] { COMPONENT_NAME };
    }
    
    public override void Update(IReadOnlyList<(EntityComponent component, bool localChange)> updated)
    {
        foreach (var (entityComponent, localChange) in updated)
        {
            _dataMap[entityComponent.EntityId] = ByteArrayToResourcePrefabData(entityComponent.Data);
            OnComponentUpdated?.Invoke(entityComponent.EntityId, _dataMap[entityComponent.EntityId]);
        }
    }
    
    public override void Delete(IReadOnlyList<(EntityComponent component, bool localChange)> deleted)
    {
        foreach (var (entityComponent, localChange) in deleted)
        {
            var entity = _session.GetEntity(entityComponent.EntityId);
            if (entity == null) continue;

            _dataMap.Remove(entity.Id);
        }
    }

    public bool SetComponent(uint entityId, ResourcePrefabData ornamentData)
    {
        var entity = _session.GetEntity(entityId);
        if (entity == null) return false;
        
        _dataMap[entityId] = ornamentData;
        
        var component = _session.GetEntityComponent(entityId, COMPONENT_NAME);
        if (component == null)
        {
            _session.AddComponent(
                COMPONENT_NAME,
                entityId,
                ResourcePrefabDataToByteArray(ornamentData),
                () => { },
                error => Debug.LogError(error)
            );

            return true;
        }
        else
        {
            return _session.UpdateComponent(
                COMPONENT_NAME,
                entityId,
                ResourcePrefabDataToByteArray(ornamentData)
            );
        }
    }
    
    public ResourcePrefabData GetComponent(uint entityId)
    {
        if (_session.GetEntity(entityId) == null || !_dataMap.ContainsKey(entityId))
            return null;

        return _dataMap[entityId];
    }
    
    public void DeleteComponent(uint entityId)
    {
        _session.DeleteComponent(COMPONENT_NAME, entityId, () =>
        {
            _dataMap.Remove(entityId);
        });
    }
    
    private byte[] ResourcePrefabDataToByteArray(ResourcePrefabData ornamentData)
    {
        string json = JsonUtility.ToJson(ornamentData);
        return Encoding.UTF8.GetBytes(json);
    }
    
    private ResourcePrefabData ByteArrayToResourcePrefabData(byte[] bytes)
    {
        string jsonFromBytes = Encoding.UTF8.GetString(bytes);
        return JsonUtility.FromJson<ResourcePrefabData>(jsonFromBytes);
    }
}