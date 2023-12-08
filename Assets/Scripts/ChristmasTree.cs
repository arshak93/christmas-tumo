
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    [SerializeField] private List<Transform> ornamentPositions = new List<Transform>();
    [SerializeField] private List<Material> ornamentMaterials = new List<Material>();
    [SerializeField] private List<Ornament> ornamentPrefabs;
    
    private void Start()
    {
        foreach (var ornamentPosition in ornamentPositions)
        {
            ornamentPosition.gameObject.AddComponent<OrnamentPosition>();
        }
    }
}