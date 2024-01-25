using System;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Image selectionBox;
    
    public event Action OnButtonClick;

    private void Start()
    {
        SetSelected(false);
    }

    public void SetColor(Color color)
    {
        rawImage.color = color;
    }

    public void SetTexture(Texture texture)
    {
        rawImage.texture = texture;
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
