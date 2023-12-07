using UnityEngine;

public class Ornament : MonoBehaviour
{
    [SerializeField] private Renderer bodyRenderer;
    [SerializeField] private Rigidbody sphereRigidbody;
    [SerializeField] private Sprite screenshot;

    public Renderer BodyRenderer => bodyRenderer;
    public Rigidbody SphereRigidbody => sphereRigidbody;
    public Sprite Screenshot => screenshot;
}