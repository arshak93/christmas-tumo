using Posemesh.Pocketbase.Model;
using UnityEngine;

public class OrnamentPosition : MonoBehaviour
{
    public enum Type
    {
        Regular,
        Top
    }

    [SerializeField] private Type type;
    
    private ChristmasTree _tree;
    private int _positionIndex = -1;
    private Ornament _attachedOrnament;
    private OrnamentData _attachedOrnamentData;

    public OrnamentData AttachedOrnamentData
    {
        get
        {
            return _attachedOrnamentData;
        }
        set
        {
            RemoveOrnament();
            
            _attachedOrnamentData = value;
            _attachedOrnamentData.positionIndex = _positionIndex;
            Ornament ornamentPrefab = Resources.Load<Ornament>($"Ornaments/{_attachedOrnamentData.prefab}");
            _attachedOrnament = Instantiate(ornamentPrefab, this.transform);
            
            if(ornamentPrefab.SupportedMaterials != Ornament.SupportedMaterialType.None)
                _attachedOrnament.SetMaterial(Resources.Load<Material>($"Materials/{ornamentPrefab.SupportedMaterials}/{_attachedOrnamentData.material}"));
        }
    }

    public Type PositionType=> type;
    public ChristmasTree Tree => _tree;
    public bool HasOrnament => _attachedOrnamentData != null;

    public void Initialize(ChristmasTree tree, int index)
    {
        _tree = tree;
        _positionIndex = index;

        if (HasOrnament)
            _attachedOrnamentData.positionIndex = _positionIndex;
    }

    public void RemoveOrnament()
    {
        _attachedOrnamentData = null;
        
        if(_attachedOrnament != null)
            Destroy(_attachedOrnament.gameObject);
    }
}