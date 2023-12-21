using System;
using Auki.ConjureKit;
using Unity.XR.CoreUtils;
using UnityEngine;

public class EntityGameObject : MonoBehaviour
{
    [SerializeField] bool useEntityPose;

    public uint EntityId { get; private set; }
    public bool? IsOwn { get; private set; }

    public event Action<ConjureKitWrapper> OnEntityReady;

    private ConjureKitWrapper _conjureKitWrapper;
    private IConjureKit _conjureKit;

    public void Init(uint entityId)
    {
        EntityId = entityId;
        IsOwn = false;
    }

    private void Start()
    {
        _conjureKitWrapper = FindObjectOfType<ConjureKitWrapper>();

        if (_conjureKitWrapper == null)
        {
            enabled = false;
            return;
        }
        else
        {
            _conjureKit = _conjureKitWrapper.ConjureKit;
            // More CK checks here?
        }

        if (IsOwn == null)
        {
            IsOwn = true;
            _conjureKit.GetSession().AddEntity(
                entity =>
                {
                    EntityId = entity.Id;

                    if (useEntityPose)
                    {
                        _conjureKit.GetSession().SetEntityPose(EntityId, transform.GetWorldPose());
                    }
                }, Debug.LogError);
        }

        OnEntityReady?.Invoke(_conjureKitWrapper);
    }
}