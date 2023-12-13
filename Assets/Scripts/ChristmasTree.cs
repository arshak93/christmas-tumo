
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    [SerializeField] private List<Transform> ornamentPositions = new List<Transform>();
    [SerializeField] private List<Material> ornamentMaterials = new List<Material>();
    [SerializeField] private List<Ornament> ornamentPrefabs;
    
    private void Start()
    {
        foreach (var ornamentPositionTransform in ornamentPositions)
        {
            OrnamentPosition ornamentPosition = ornamentPositionTransform.gameObject.AddComponent<OrnamentPosition>();
            
            if(PlayerPrefs.HasKey(ornamentPosition.name))
            {
                string json = PlayerPrefs.GetString(ornamentPosition.name);
                OrnamentData ornamentData = JsonUtility.FromJson<OrnamentData>(json);
                ornamentPosition.AttachedOrnamentData = ornamentData;
            }
        }
    }
}