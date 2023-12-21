using System;

namespace Multiplayer
{
    [Serializable]
    public class ResourcePrefabData
    {
        public string prefabAddress;

        public ResourcePrefabData()
        {
        }

        public ResourcePrefabData(string prefabAddress)
        {
            this.prefabAddress = prefabAddress;
        }
    }
}