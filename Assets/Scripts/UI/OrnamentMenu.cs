using UnityEngine;

public class OrnamentMenu : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}