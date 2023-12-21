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

        public ChristmasTreeData()
        {
        }

        public ChristmasTreeData(string domainId, SerializablePose pose, TreeData data)
        {
            this.domainId = domainId;
            this.pose = pose;
            this.data = data;
        }
    }
}