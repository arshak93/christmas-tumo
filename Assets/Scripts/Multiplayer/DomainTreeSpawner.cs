using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer.Model;
using Multiplayer.Pocketbase;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Multiplayer
{
    public class DomainTreeSpawner : MonoBehaviour
    {
        [SerializeField] private PocketbaseApiClient pocketbaseClient;

        private string _domainId = "712cc24b-99bc-4079-b7d1-6b227b3fde72";

        private Dictionary<string, ChristmasTree> _christmasTrees = new Dictionary<string, ChristmasTree>();

        private void Start()
        {
            pocketbaseClient.GetTreesInDomain(_domainId, trees =>
            {
                foreach (var tree in trees)
                {
                    ChristmasTree christmasTree = Instantiate(Resources.Load<ChristmasTree>(tree.data.prefabAddress));
                    christmasTree.transform.SetWorldPose(tree.pose.ToPose());
                    christmasTree.SetTreeData(tree.data);
                    _christmasTrees.Add(tree.id, christmasTree);
                }
            });
        }

        public void AddNewTree(ChristmasTree tree)
        {
            if(_christmasTrees.ContainsValue(tree))
                return;
            
            SerializablePose pose = SerializablePose.FromPose(tree.transform.GetWorldPose());
            TreeData treeData = tree.GetTreeData();

            pocketbaseClient.AddTreeToDomain(_domainId, pose, treeData, christmasTreeData =>
            {
                _christmasTrees.Add(christmasTreeData.id, tree);
            });
        }
    }
}