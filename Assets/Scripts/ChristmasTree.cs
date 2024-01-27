using System.Collections.Generic;
using Posemesh.Pocketbase.Model;
using UnityEngine;
using UnityEngine.Serialization;

public class ChristmasTree : MonoBehaviour
{
    [SerializeField] private string prefabAddress;
    [SerializeField] private List<OrnamentPosition> ornamentPositions = new List<OrnamentPosition>();

    [FormerlySerializedAs("Id")] [HideInInspector]
    public string id;

    private void Start()
    {
        for (var i = 0; i < ornamentPositions.Count; i++)
        {
            ornamentPositions[i].Initialize(this, i);
        }
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