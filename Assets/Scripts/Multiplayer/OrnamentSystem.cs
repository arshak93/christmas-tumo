using System;
using System.Collections.Generic;
using System.Text;
using Auki.ConjureKit;
using Auki.ConjureKit.ECS;
using UnityEngine;

public class OrnamentSystem : SystemBase
{
    private const string ORNAMENT_COMPONENT_NAME = "ornament";
    
    public event Action<uint, OrnamentData> OnOrnamentComponentUpdated;
    
    private readonly IDictionary<uint, OrnamentData> _entityOrnamentDataMap = new Dictionary<uint, OrnamentData>();

    public OrnamentSystem(Session session) : base(session)
    {
    }
    
    public override string[] GetComponentTypeNames()
    {
        return new[] { ORNAMENT_COMPONENT_NAME };
    }
    
    public override void Update(IReadOnlyList<(EntityComponent component, bool localChange)> updated)
    {
        foreach (var (entityComponent, localChange) in updated)
        {
            _entityOrnamentDataMap[entityComponent.EntityId] = ByteArrayToOrnamentData(entityComponent.Data);
            OnOrnamentComponentUpdated?.Invoke(entityComponent.EntityId, _entityOrnamentDataMap[entityComponent.EntityId]);
        }
    }
    
    public override void Delete(IReadOnlyList<(EntityComponent component, bool localChange)> deleted)
    {
        foreach (var (entityComponent, localChange) in deleted)
        {
            var entity = _session.GetEntity(entityComponent.EntityId);
            if (entity == null) continue;

            _entityOrnamentDataMap.Remove(entity.Id);
        }
    }

    public bool SetOrnamentComponent(uint entityId, OrnamentData ornamentData)
    {
        var entity = _session.GetEntity(entityId);
        if (entity == null) return false;
        
        _entityOrnamentDataMap[entityId] = ornamentData;
        
        var component = _session.GetEntityComponent(entityId, ORNAMENT_COMPONENT_NAME);
        if (component == null)
        {
            _session.AddComponent(
                ORNAMENT_COMPONENT_NAME,
                entityId,
                OrnamentDataToByteArray(ornamentData),
                () => { },
                error => Debug.LogError(error)
            );

            return true;
        }
        else
        {
            return _session.UpdateComponent(
                ORNAMENT_COMPONENT_NAME,
                entityId,
                OrnamentDataToByteArray(ornamentData)
            );
        }
    }
    
    public OrnamentData GetOrnamentComponent(uint entityId)
    {
        if (_session.GetEntity(entityId) == null || !_entityOrnamentDataMap.ContainsKey(entityId))
            return null;

        return _entityOrnamentDataMap[entityId];
    }
    
    public void DeleteOrnamentComponent(uint entityId)
    {
        _session.DeleteComponent(ORNAMENT_COMPONENT_NAME, entityId, () =>
        {
            _entityOrnamentDataMap.Remove(entityId);
        });
    }
    
    private byte[] OrnamentDataToByteArray(OrnamentData ornamentData)
    {
        string json = JsonUtility.ToJson(ornamentData);
        return Encoding.UTF8.GetBytes(json);
    }
    
    private OrnamentData ByteArrayToOrnamentData(byte[] bytes)
    {
        string jsonFromBytes = Encoding.UTF8.GetString(bytes);
        return JsonUtility.FromJson<OrnamentData>(jsonFromBytes);
    }
}