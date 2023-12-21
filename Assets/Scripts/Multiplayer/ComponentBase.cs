using System;
using UnityEngine;

namespace Multiplayer
{
    [RequireComponent(typeof(EntityGameObject))]
    public abstract class ComponentBase : MonoBehaviour
    {
        protected EntityGameObject EntityGameObject;

        private void Start()
        {
            EntityGameObject = GetComponent<EntityGameObject>();

            EntityGameObject.OnEntityReady += OnEntityReady;
        }

        protected abstract void OnEntityReady(ConjureKitWrapper conjureKitWrapper);
    }
}