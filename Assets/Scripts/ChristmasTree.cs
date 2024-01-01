using System.Collections.Generic;
using Multiplayer;
using Multiplayer.Model;
using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    [SerializeField] private string prefabAddress;
    [SerializeField] private List<OrnamentPosition> ornamentPositions = new List<OrnamentPosition>();

    // private DomainTreeSpawner _domainTreeSpawner;

    private void Start()
    {
        // _domainTreeSpawner = FindObjectOfType<DomainTreeSpawner>();
        // if (_domainTreeSpawner == null)
        // {
        //     Debug.LogError($"DomainTreeSpawner required by ChristmasTree script on game object {gameObject.name}");
        //     return;
        // }
        
        var ornaments = Resources.LoadAll<Ornament>("Ornaments");

        for (var i = 0; i < ornamentPositions.Count; i++)
        {
            ornamentPositions[i].Initialize(this, i);

            if (Random.value < 0.8f)
            {
                var ornament = ornaments[Random.Range(0, ornaments.Length)];
                Instantiate(ornament, ornamentPositions[i].transform);
                ornamentPositions[i].GetComponentInChildren<Collider>().enabled = false;
            }
        }
        
        // _domainTreeSpawner.AddNewTree(this);
    }

    public TreeData GetTreeData()
    {
        List<OrnamentData> ornaments = new List<OrnamentData>();

        for (int i = 0; i < ornamentPositions.Count; i++)
        {
            if(ornamentPositions[i].AttachedOrnamentData != null)
                ornaments.Add(ornamentPositions[i].AttachedOrnamentData);
        }
        
        TreeData treeData = new TreeData(prefabAddress, ornaments);

        return treeData;
    }

    public void SetTreeData(TreeData treeData)
    {
        foreach (var ornamentData in treeData.ornaments)
        {
            ornamentPositions[ornamentData.positionIndex].AttachedOrnamentData =
                ornamentData;
        }
    }
}