using UnityEngine;

public class Ornament : MonoBehaviour
{
    public enum SupportedMaterialType
    {
        None,
        Opaque,
        Transparent
    }
    
    [SerializeField] private Renderer bodyRenderer;
    [SerializeField] private Sprite screenshot;
    [SerializeField] private SupportedMaterialType supportedMaterials;
    [SerializeField] private OrnamentPosition.Type positionType;

    public Sprite Screenshot => screenshot;
    public SupportedMaterialType SupportedMaterials => supportedMaterials;
    public OrnamentPosition.Type PositionType => positionType;

    public void SetMaterial(Material material)
    {
        bodyRenderer.sharedMaterial = material;
    }
}