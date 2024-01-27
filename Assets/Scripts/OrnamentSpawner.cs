using System.Collections.Generic;
using Multiplayer;
using UI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// OrnamentSpawner handles the logic of selecting and hanging an ornament from the tree
/// </summary>
public class OrnamentSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private OrnamentButton ornamentButtonPrefab;
    [SerializeField] private MaterialButton materialButtonPrefab;
    [SerializeField] private Transform ornamentButtonContent;
    [SerializeField] private Transform materialButtonContent;
    [SerializeField] private DomainTreeSpawner domainTreeSpawner;
    
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button placeButton;
    [SerializeField] private Confetti confetti;

    private Ornament[] _ornamentPrefabs;
    // Selected ornament to spawn
    private Ornament _selectedOrnamentPreview;
    // Selected ornament to spawn data
    private OrnamentData _selectedOrnamentToSpawnData = new OrnamentData();
    // The ornament position we currently point to
    private OrnamentPosition _selectedOrnamentPosition;

    private List<OrnamentButton> _ornamentButtons = new List<OrnamentButton>();
    private List<MaterialButton> _materialButtons = new List<MaterialButton>();
    
    void Start()
    {
        // Load all ornaments
        _ornamentPrefabs = Resources.LoadAll<Ornament>("Ornaments");
            
        // Create the ornament buttons
        foreach (var ornament in _ornamentPrefabs)
        {
            OrnamentButton button = Instantiate(ornamentButtonPrefab, ornamentButtonContent);
            _ornamentButtons.Add(button);
            button.gameObject.SetActive(true);
            button.SetScreenshot(ornament.Screenshot);
            button.OnButtonClick += () =>
            {
                SelectOrnamentPrefab(ornament);
                button.SetSelected(true);
            };
        }
        
        placeButton.onClick.AddListener(OnPlaceButtonClick);
        deleteButton.onClick.AddListener(OnDeleteButtonClick);
    }

    private void OnPlaceButtonClick()
    {
        // If the position is valid, place the ornament
        if (_selectedOrnamentPosition != null && !_selectedOrnamentPosition.HasOrnament)
        {
            PlaceSelectedOrnament();
        }
    }

    private void OnDeleteButtonClick()
    {
        // If the position is valid and has an ornament, remove it
        if (_selectedOrnamentPosition != null && _selectedOrnamentPosition.HasOrnament)
        {
            DeleteOrnamentAtSelectedPosition();
        }
    }
    
    private void SelectOrnamentPrefab(Ornament ornamentPrefab)
    {
        // Selected a new ornament
        if (_selectedOrnamentToSpawnData.prefab != ornamentPrefab.name)
        {
            ClearCurrentSelection();
            _selectedOrnamentToSpawnData.prefab = ornamentPrefab.name;
            _selectedOrnamentPreview = Instantiate(ornamentPrefab);
            
            // Match the scale of the tree prefabs
            _selectedOrnamentPreview.transform.localScale = Vector3.one * 0.3f;
            
            // Setup material selector if the ornament supports material change
            if (_selectedOrnamentPreview.SupportedMaterials != Ornament.SupportedMaterialType.None)
            {
                Debug.Log($"Materials/{_selectedOrnamentPreview.SupportedMaterials}");
                // Load opaque or transparent materials based on the ornament material type
                var ornamentMaterials =
                    Resources.LoadAll<Material>($"Materials/{_selectedOrnamentPreview.SupportedMaterials}");
                
                // Create the material buttons
                foreach (var material in ornamentMaterials)
                {
                    MaterialButton button = Instantiate(materialButtonPrefab, materialButtonContent);
                    _materialButtons.Add(button);
                    button.gameObject.SetActive(true);

                    // Set the button texture or color
                    if (_selectedOrnamentPreview.SupportedMaterials == Ornament.SupportedMaterialType.Opaque)
                        button.SetTexture(material.mainTexture);
                    else
                        button.SetColor(material.color);                        

                    button.OnButtonClick += () =>
                    {
                        UnselectAllMaterialButtons();
                        
                        // Select new Material
                        SelectOrnamentMaterial(material);
                        button.SetSelected(true);
                    };
                }
                
                // Select the first material
                SelectOrnamentMaterial(ornamentMaterials[0]);
                _materialButtons[0].SetSelected(true);
            }
        }
    }

    private void UnselectAllOrnamentButtons()
    {
        foreach (var ornamentButton in _ornamentButtons)
            ornamentButton.SetSelected(false);
    }
    
    private void UnselectAllMaterialButtons()
    {
        foreach (var materialButton in _materialButtons)
            materialButton.SetSelected(false);
    }

    private void SelectOrnamentMaterial(Material material)
    {
        _selectedOrnamentToSpawnData.material = material.name;
        _selectedOrnamentPreview.SetMaterial(material);
    }

    private void ClearCurrentSelection()
    {
        // Delete the old ornament preview
        _selectedOrnamentToSpawnData.prefab = null;
        if (_selectedOrnamentPreview != null)
        {
            Destroy(_selectedOrnamentPreview.gameObject);
        }
        _selectedOrnamentToSpawnData = new OrnamentData();

        UnselectAllOrnamentButtons();
        
        // Clear the material buttons
        foreach (var materialButton in _materialButtons)
        {
            Destroy(materialButton.gameObject);
        }
        _materialButtons.Clear();
    }

    private void PlaceSelectedOrnament()
    {
        // Place the new ornament in the selected position, spawn confetti effect and delete the preview
        _selectedOrnamentPosition.AttachedOrnamentData = _selectedOrnamentToSpawnData;
        Instantiate(confetti, _selectedOrnamentPosition.transform);
        ClearCurrentSelection();
        
        // Update the tree data on the backend
        domainTreeSpawner.UpdateTree(_selectedOrnamentPosition.Tree);
    }

    private void DeleteOrnamentAtSelectedPosition()
    {
        _selectedOrnamentPosition.RemoveOrnament();
        
        // Update the tree data on the backend
        domainTreeSpawner.UpdateTree(_selectedOrnamentPosition.Tree);
    }

    private void Update()
    {
        // If an ornament is selected position the preview in front of the camera
        if (_selectedOrnamentPreview != null)
        {
            _selectedOrnamentPreview.transform.position =
                mainCamera.transform.position +
                mainCamera.transform.forward * 0.8f;
        }

        // Reset the previously selected ornament position and disable the buttons
        _selectedOrnamentPosition = null;
        placeButton.interactable = false;
        deleteButton.interactable = false;
        
        // Calculate the center of the viewport
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Create a ray from the camera through the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        
        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            // Check if we are pointing at ornament position on a tree
            if (hit.collider.CompareTag("OrnamentPosition"))
            {
                OrnamentPosition ornamentPosition = hit.collider.GetComponent<OrnamentPosition>();
                _selectedOrnamentPosition = ornamentPosition;

                // If there's already an ornament here give the option to delete it, if not allow placing a new one
                if (ornamentPosition.HasOrnament)
                {
                    deleteButton.interactable = true;
                }
                else if(_selectedOrnamentPreview != null && _selectedOrnamentPreview.PositionType == ornamentPosition.PositionType)
                {
                    placeButton.interactable = true;
                    _selectedOrnamentPreview.transform.position = hit.collider.transform.position;
                }
            }
        }
    }
}
