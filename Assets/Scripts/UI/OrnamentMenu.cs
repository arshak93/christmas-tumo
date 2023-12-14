using System;
using UnityEngine;
using UnityEngine.UI;

public class OrnamentMenu : MonoBehaviour
{
    [SerializeField] private OrnamentSpawner ornamentSpawner;
    [SerializeField] private Button placeButton;

    private void Start()
    {
        ornamentSpawner.OnSelectedOrnamentPositionChanged += OnOrnamentPositionChanged;
    }

    private void OnOrnamentPositionChanged(OrnamentPosition selectedOrnamentPosition)
    {
        if (selectedOrnamentPosition == null)
        {
            placeButton.interactable = false;
        }
        else
        {
            placeButton.interactable = true;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}