using UnityEngine;

public class ContinuousMaterialChanger : MonoBehaviour
{
    public enum Type
    {
        Static,
        Hertov,
        Mekumech,
    }

    public Type type;
    public Material[] materials;
    public Renderer[] lights;
    public int a = 0;
    public float timeBetweenChanges = 2f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timeBetweenChanges)
        {
            for (int i = 0;i<lights.Length ;i++)
            {
                if (type == Type.Static)
                {
                    lights[i].sharedMaterial = materials[a % materials.Length];
                }
                else if (type == Type.Hertov)
                {
                    lights[i].sharedMaterial = materials[(i + a) % materials.Length];
                }
                else if (type == Type.Mekumech)
                {
                    lights[i].sharedMaterial = materials[((i % 2) + a) % materials.Length];
                }
            }


            a++;
            

            _timer = 0f;
        }
    }

    private void ChangeMaterial(Renderer renderer)
    {
        if (renderer != null && materials.Length > 0)
        {
            Material newMaterial = materials[a];
            renderer.material = newMaterial;
        }
    }
}