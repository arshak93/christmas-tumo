using System.Collections.Generic;
using UnityEngine;

public class OrnamentSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform environment;
    [SerializeField] private List<Ornament> ornamentPrefabs;
    [SerializeField] private List<Material> ornamentMaterials;
    [SerializeField] private OrnamentButton ornamentButtonPrefab;
    [SerializeField] private ColorButton colorButtonPrefab;
    [SerializeField] private Transform ornamentButtonContent;
    [SerializeField] private Transform colorButtonContent;
    
    // Selected ornament to spawn
    private Ornament _selectedOrnamentToSpawn;
    // Selected ornament to spawn data
    private OrnamentData _selectedOrnamentToSpawnData = new OrnamentData();
    // The ornament position we currently point to
    private OrnamentPosition _selectedOrnamentPosition;
    
    void Start()
    {
        // Create the ornament buttons
        foreach (var ornament in ornamentPrefabs)
        {
            OrnamentButton button = Instantiate(ornamentButtonPrefab, ornamentButtonContent);
            button.SetScreenshot(ornament.Screenshot);
            button.OnButtonClick += () => SelectOrnamentPrefab(ornament);
        }
        
        // Disable the ornament button prefab
        ornamentButtonPrefab.gameObject.SetActive(false);
        
        // Create the color buttons
        foreach (var material in ornamentMaterials)
        {
            ColorButton button = Instantiate(colorButtonPrefab, colorButtonContent);
            button.SetColor(material.color);

            button.OnButtonClick += () => SelectOrnamentMaterial(material);
        }
        
        // Disable the ornament button prefab
        colorButtonPrefab.gameObject.SetActive(false);
        
        // Start with the first ornament and the first color selected
        SelectOrnamentPrefab(ornamentPrefabs[0]);
        SelectOrnamentMaterial(ornamentMaterials[0]);
    }
    
    private void SelectOrnamentPrefab(Ornament ornamentPrefab)
    {
        // Tapped on the same selected button again
        if (_selectedOrnamentToSpawnData.prefab == ornamentPrefab.name)
        {
            Debug.Log(_selectedOrnamentPosition);
            // If the position is valid, place the ornament
            if (_selectedOrnamentPosition != null)
            {
                _selectedOrnamentPosition.AttachedOrnamentData = _selectedOrnamentToSpawnData;
            }
        }
        else // Selected a new ornament
        {
            ClearCurrentSelection();
            _selectedOrnamentToSpawnData.prefab = ornamentPrefab.name;
            _selectedOrnamentToSpawnData.text = ornamentPrefab.text;
            _selectedOrnamentToSpawn = Instantiate(ornamentPrefab, environment);
        }
    }
    
    private void SelectOrnamentMaterial(Material material)
    {
        _selectedOrnamentToSpawnData.material = material.name;
        _selectedOrnamentToSpawn.SetMaterial(material);
    }

    private void ClearCurrentSelection()
    {
        _selectedOrnamentToSpawnData.prefab = null;
        if (_selectedOrnamentToSpawn != null)
        {
            Destroy(_selectedOrnamentToSpawn.gameObject);
        }
    }

    private void Update()
    {
        if(_selectedOrnamentToSpawn == null)
            return;

        _selectedOrnamentToSpawn.transform.position =
            mainCamera.transform.position + 
            mainCamera.transform.forward * 0.8f;

        _selectedOrnamentPosition = null;


        // Calculate the center of the viewport
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Create a ray from the camera through the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        
        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("OrnamentPosition"))
            {
                OrnamentPosition ornamentPosition = hit.collider.GetComponent<OrnamentPosition>();

                if (!ornamentPosition.HasOrnament)
                {
                    _selectedOrnamentPosition = ornamentPosition;
                    _selectedOrnamentToSpawn.transform.position = hit.collider.transform.position;
                }
            }
        }
    }
}
