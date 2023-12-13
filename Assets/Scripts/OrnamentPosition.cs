using UnityEngine;

public class OrnamentPosition : MonoBehaviour
{
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
            _attachedOrnamentData = value;
            Ornament ornamentPrefab = Resources.Load<Ornament>("Ornaments/" + _attachedOrnamentData.prefab);
            _attachedOrnament = Instantiate(ornamentPrefab, this.transform);
            _attachedOrnament.SetMaterial(Resources.Load<Material>("Materials/" + _attachedOrnamentData.material));
            _attachedOrnament.text = _attachedOrnamentData.text;
        }
    }

    public bool HasOrnament => _attachedOrnamentData != null;

    public void RemoveOrnament()
    {
        _attachedOrnamentData = null;
        Destroy(_attachedOrnament.gameObject);
    }
}