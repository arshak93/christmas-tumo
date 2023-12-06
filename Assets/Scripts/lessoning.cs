using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lessoning : MonoBehaviour
{
    public List<GameObject> testList;
    public List<GameObject> positions;
    void Start()
    {
        for(int i=0; i<positions.Count; i++){

            int randchoice = Random.Range(0, testList.Count);
            Instantiate(testList[randchoice], positions[i].GetComponent<Transform>().position, Quaternion.identity);
        }
    }
}
