using System;

namespace Multiplayer.Model
{
    [Serializable]
    public class ChristmasTreeData
    {
        public string id;
        public string domainId;
        public SerializablePose pose;
        public TreeData data;
    }
}