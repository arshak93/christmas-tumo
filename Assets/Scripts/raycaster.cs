using UnityEngine;
using System.Collections.Generic;
public class raycaster : MonoBehaviour
{
    public Camera camera;
    public List<GameObject> lists;
    public int spawnCount = 5;
    private int i;

    public void Three()
    {
        i = 3;
    }
    public void Zero()
    {
        i = 0;
    }
    public void One()
    {
        i = 1;
    }
    public void Two()
    {
        i = 2;
    }
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = camera.ScreenPointToRay(mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                GameObject hitObject = hitInfo.collider.gameObject;

                Debug.Log("Hit object: " + hitObject.name);

                GameObject element = lists[(int)Random.Range(0, lists.Count)];

                Instantiate(lists[i], hitObject.transform.position, Quaternion.identity);

            }
            else
            {
                Debug.Log("Ray did not hit anything.");
            }
        }
    }
}