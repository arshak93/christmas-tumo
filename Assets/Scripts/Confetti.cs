using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] float particleDuration = 1f;

    public void Start()
    {
        particle.Play();
        Invoke(nameof(DestroyConfetti), particleDuration);
    }

    private void DestroyConfetti()
    {
        Destroy(gameObject);
    }
}
