using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrnamentButton : MonoBehaviour
{
    [SerializeField] private Image screenshotImage;
    [SerializeField] private Image selectionBox;

    public event Action OnButtonClick;
    
    private void Start()
    {
        SetSelected(false);
    }

    public void SetScreenshot(Sprite sprite)
    {
        screenshotImage.sprite = sprite;
    }

    public void OnClick()
    {
        OnButtonClick?.Invoke();
    }

    public void SetSelected(bool isSelected)
    {
        selectionBox.gameObject.SetActive(isSelected);
    }
}
