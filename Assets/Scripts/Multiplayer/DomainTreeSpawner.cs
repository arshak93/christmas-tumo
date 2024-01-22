using System.Collections.Generic;
using System.Linq;
using Multiplayer.Pocketbase;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Multiplayer
{
    public class DomainTreeSpawner : MonoBehaviour
    {
        [SerializeField] private ConjureKitWrapper conjureKitWrapper;
        [SerializeField] private PocketbaseApiClient pocketbaseClient;

        private string _domainId = "712cc24b-99bc-4079-b7d1-6b227b3fde72";

        private Dictionary<string, ChristmasTree> _christmasTrees = new Dictionary<string, ChristmasTree>();

        private void Start()
        {
            conjureKitWrapper.OnDomainEntered += OnDomainEntered; 
        }

        private void OnDomainEntered(string domainId)
        {
            _domainId = domainId;
            
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

        public void UpdateTree(ChristmasTree tree)
        {
            if (!_christmasTrees.ContainsValue(tree))
                return;

            var treeId = _christmasTrees.First(entry => entry.Value == tree).Key;
            pocketbaseClient.UpdateTree(
                treeId, 
                tree.transform.GetWorldPose(), 
                tree.GetTreeData(),
                christmasTreeData =>
                {
                    Debug.Log("Tree updated successfully!");
                });
        }
    }
}