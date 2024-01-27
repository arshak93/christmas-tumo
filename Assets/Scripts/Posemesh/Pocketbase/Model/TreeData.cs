using System;
using System.Collections.Generic;

namespace Posemesh.Pocketbase.Model
{
    [Serializable]
    public class TreeData
    {
        public string prefabAddress;
        public List<OrnamentData> ornaments;

        public TreeData()
        {
        }

        public TreeData(string prefabAddress, List<OrnamentData> ornaments)
        {
            this.prefabAddress = prefabAddress;
            this.ornaments = ornaments;
        }
    }
}