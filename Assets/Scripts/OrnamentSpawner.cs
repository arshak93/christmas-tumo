using System;
using System.Collections.Generic;
using UnityEngine;

public class OrnamentSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Ornament> ornamentPrefabs;
    [SerializeField] private OrnamentButton buttonPrefab;
    [SerializeField] private Transform content;

    private Ornament selectedOrnament;
    
    void Start()
    {
        foreach (var ornament in ornamentPrefabs)
        {
            OrnamentButton button = Instantiate(buttonPrefab, content);
            button.SetScreenshot(ornament.Screenshot);
            button.OnButtonClick += () => SelectOrnamentPrefab(ornament);
        }
        
        buttonPrefab.gameObject.SetActive(false);
    }

    private void SelectOrnamentPrefab(Ornament ornamentPrefab)
    {
        if (selectedOrnament != null)
        {
            Destroy(selectedOrnament.gameObject);
        }
        
        selectedOrnament = Instantiate(ornamentPrefab);
    }

    private void Update()
    {
        if(selectedOrnament == null)
            return;

        selectedOrnament.transform.position =
            mainCamera.transform.position +
            mainCamera.transform.forward * 2;


        // Calculate the center of the viewport
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Create a ray from the camera through the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);

        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {

            if (hit.collider.CompareTag("OrnamentPosition"))
            {
                selectedOrnament.transform.position = hit.collider.transform.position;
            }
        }
    }
}
