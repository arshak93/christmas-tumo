using System.Collections.Generic;
using UnityEngine;

public class OrnamentSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Ornament> ornamentPrefabs;

    [SerializeField] private OrnamentButton buttonPrefab;
    [SerializeField] private Transform content;
    
    void Start()
    {
        foreach (var ornament in ornamentPrefabs)
        {
            OrnamentButton button = Instantiate(buttonPrefab, content);
            button.SetScreenshot(ornament.Screenshot);
        }
    }
    
   
}
