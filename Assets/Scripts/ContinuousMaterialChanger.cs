using UnityEngine;

public class ContinuousMaterialChanger : MonoBehaviour
{
    public enum Type
    {
        Static,
        Sequence,
        EverySecondLight,
    }

    public Type type;
    public Material[] materials;
    public Renderer[] lights;
    public float timeBetweenChanges = 2f;

    private int _index;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timeBetweenChanges)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (type == Type.Static)
                {
                    lights[i].sharedMaterial = materials[_index % materials.Length];
                }
                else if (type == Type.Sequence)
                {
                    lights[i].sharedMaterial = materials[(i + _index) % materials.Length];
                }
                else if (type == Type.EverySecondLight)
                {
                    lights[i].sharedMaterial = materials[((i % 2) + _index) % materials.Length];
                }
            }


            _index++;


            _timer = 0f;
        }
    }
}