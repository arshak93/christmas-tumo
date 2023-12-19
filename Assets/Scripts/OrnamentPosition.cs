using UnityEngine;

public class OrnamentPosition : MonoBehaviour
{
    private int _positionIndex = -1;
    private OrnamentSystem _ornamentSystem;
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
            Ornament ornamentPrefab = Resources.Load<Ornament>("Ornaments/" + _attachedOrnamentData.prefab);
            _attachedOrnament = Instantiate(ornamentPrefab, this.transform);
            _attachedOrnament.SetMaterial(Resources.Load<Material>("Materials/" + _attachedOrnamentData.material));
            _attachedOrnament.text = _attachedOrnamentData.text;

            string json = JsonUtility.ToJson(_attachedOrnamentData);
            PlayerPrefs.SetString(gameObject.name, json);
            PlayerPrefs.Save();
        }
    }

    public bool HasOrnament => _attachedOrnamentData != null;

    public void Initialize(int index, OrnamentSystem ornamentSystem)
    {
        _positionIndex = index;
        _ornamentSystem = ornamentSystem;
        
        _ornamentSystem.OnOrnamentComponentUpdated += OnOrnamentComponentUpdated;
    }

    private void OnOrnamentComponentUpdated(uint entityId, OrnamentData ornamentData)
    {
        if (ornamentData.positionIndex == _positionIndex)
        {
            AttachedOrnamentData = ornamentData;
        }
    }

    public void RemoveOrnament()
    {
        _attachedOrnamentData = null;
        Destroy(_attachedOrnament.gameObject);
    }
}