
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    [SerializeField] private ConjureKitWrapper conjureKitWrapper;
    [SerializeField] private List<Transform> ornamentPositions = new List<Transform>();

    private void Start()
    {
        for (var i = 0; i < ornamentPositions.Count; i++)
        {
            var ornamentPositionTransform = ornamentPositions[i];
            OrnamentPosition ornamentPosition = ornamentPositionTransform.gameObject.AddComponent<OrnamentPosition>();
            ornamentPosition.Initialize(i, conjureKitWrapper.OrnamentSystem);

            if (PlayerPrefs.HasKey(ornamentPosition.name))
            {
                string json = PlayerPrefs.GetString(ornamentPosition.name);
                OrnamentData ornamentData = JsonUtility.FromJson<OrnamentData>(json);
                ornamentPosition.AttachedOrnamentData = ornamentData;
            }
        }
    }
}