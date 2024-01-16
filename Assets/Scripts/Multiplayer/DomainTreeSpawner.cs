using System.Collections.Generic;
using System.Linq;
using Multiplayer.Pocketbase;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace Multiplayer
{
    public class DomainTreeSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectSpawner objectSpawner;
        [SerializeField] XRInteractionGroup interactionGroup;
        [SerializeField] Button deleteButton;
        [SerializeField] private PocketbaseApiClient pocketbaseClient;

        private string _domainId = "712cc24b-99bc-4079-b7d1-6b227b3fde72";

        private Dictionary<string, ChristmasTree> _christmasTrees = new Dictionary<string, ChristmasTree>();

        private void Start()
        {
            deleteButton.onClick.AddListener(OnDeleteButtonClicked);
            objectSpawner.objectSpawned += OnTreeSpawned;
            
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

        private void OnDeleteButtonClicked()
        {
            Debug.Log(interactionGroup.focusInteractable);
            if (interactionGroup.focusInteractable == null)
                return;

            var tree = interactionGroup.focusInteractable.transform.GetComponent<ChristmasTree>();
            
            if(tree != null)
                DeleteTree(tree);
        }

        private void OnTreeSpawned(GameObject newTree)
        {
            var tree = newTree.GetComponent<ChristmasTree>();
            if(tree != null)
                AddNewTree(tree);
        }

        public void AddNewTree(ChristmasTree tree)
        {
            if (_christmasTrees.ContainsValue(tree))
                return;

            pocketbaseClient.AddTreeToDomain(
                _domainId,
                tree.transform.GetWorldPose(),
                tree.GetTreeData(),
                christmasTreeData =>
                {
                    _christmasTrees.Add(christmasTreeData.id, tree);
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
        
        public void DeleteTree(ChristmasTree tree)
        {
            Destroy(tree.gameObject);
            
            if (!_christmasTrees.ContainsValue(tree))
                return;

            var treeId = _christmasTrees.First(entry => entry.Value == tree).Key;
            pocketbaseClient.DeleteTree(
                treeId,
                () =>
                {
                    Debug.Log("Tree removed successfully!");
                    _christmasTrees.Remove(treeId);
                });
        }
    }
}