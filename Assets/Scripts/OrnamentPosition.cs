using UnityEngine;

public class OrnamentPosition : MonoBehaviour
{
    private string _attachedOrnamentName;

    public string AttachedOrnamentName
    {
        get
        {
            return _attachedOrnamentName;
        }
        set
        {
            _attachedOrnamentName = value;
            Ornament ornamentPrefab = Resources.Load<Ornament>("Ornaments/" + _attachedOrnamentName);
            Instantiate(ornamentPrefab, this.transform);
        }
    }
}