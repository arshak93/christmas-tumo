using UnityEngine;
using UnityEngine.UI;

public class OrnamentButton : MonoBehaviour
{
    [SerializeField] private Image screenshotImage;
    [SerializeField] private Image checkmark;

    public void SetScreenshot(Sprite sprite)
    {
        screenshotImage.sprite = sprite;
    }
}
