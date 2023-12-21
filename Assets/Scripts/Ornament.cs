using UnityEngine;

public class Ornament : MonoBehaviour
{
    [SerializeField] private Renderer bodyRenderer;
    [SerializeField] private Rigidbody sphereRigidbody;
    [SerializeField] private Sprite screenshot;
    
    public string text;

    public void SetMaterial(Material material)
    {
        bodyRenderer.sharedMaterial = material;
    }

    public void SetData(OrnamentData data)
    {
        bodyRenderer.sharedMaterial = Resources.Load<Material>(data.material);
    }

    public Rigidbody SphereRigidbody => sphereRigidbody;
    public Sprite Screenshot => screenshot;
    public string Text => text;
}