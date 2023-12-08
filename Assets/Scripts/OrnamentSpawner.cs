using System.Collections.Generic;
using UnityEngine;

public class OrnamentSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Ornament> ornamentPrefabs;
    [SerializeField] private OrnamentButton buttonPrefab;
    [SerializeField] private Transform content;
    
    // Selected ornament to spawn
    private Transform _selectedObjectToSpawn;
    // Selected object to spawn name
    private string _selectedObjectToSpawnName;
    
    // The ornament position we currently point to
    private OrnamentPosition _currentOrnamentPosition;
    
    void Start()
    {
        // environmentButton.onClick.AddListener(() => SelectEnvironment(tnakEnvironmentPrefab));
        
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
        Debug.Log(_selectedObjectToSpawnName);
        Debug.Log(ornamentPrefab.name);
        Debug.Log(_selectedObjectToSpawnName == ornamentPrefab.name);
        // Tapped on the same selected button again
        if (_selectedObjectToSpawnName == ornamentPrefab.name)
        {
            Debug.Log(_currentOrnamentPosition);
            // If the position is valid, place the ornament
            if (_currentOrnamentPosition != null)
            {
                _currentOrnamentPosition.AttachedOrnamentName = _selectedObjectToSpawnName;
            }
        }
        else // Selected a new ornament
        {
            ClearCurrentSelection();
            _selectedObjectToSpawnName = ornamentPrefab.name;
            _selectedObjectToSpawn = Instantiate(ornamentPrefab.transform);
        }
    }

    private void ClearCurrentSelection()
    {
        _selectedObjectToSpawnName = null;
        if (_selectedObjectToSpawn != null)
        {
            Destroy(_selectedObjectToSpawn.gameObject);
        }
    }

    private void Update()
    {
        if(_selectedObjectToSpawn == null)
            return;

        _selectedObjectToSpawn.position =
            mainCamera.transform.position +
            mainCamera.transform.forward * 2;

        _currentOrnamentPosition = null;


        // Calculate the center of the viewport
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Create a ray from the camera through the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        
        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("OrnamentPosition"))
            {
                _selectedObjectToSpawn.position = hit.collider.transform.position;
                _currentOrnamentPosition = hit.collider.GetComponent<OrnamentPosition>();
            }
        }
    }
}
