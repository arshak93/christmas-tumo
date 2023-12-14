using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    private Transform _selectedObjectToSpawn;

    public List<OrnamentPosition> OrnamentPositions = new List<OrnamentPosition>();

    private string _selectedObjectToSpawnName;

    public OrnamentPosition _currentOrnamentPosition;

    public void OnClick()
    {
        
    }
}
