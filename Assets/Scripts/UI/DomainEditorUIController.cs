using UnityEngine;

namespace UI
{
    public class DomainEditorUIController : MonoBehaviour
    {
        [SerializeField] private ConjureKitWrapper conjureKitWrapper;
        [SerializeField] private GameObject scanQRMenu;
        [SerializeField] private GameObject mainMenu;

        private void Start()
        {
            conjureKitWrapper.OnDomainEntered += OnDomainEntered;
        }

        private void OnDomainEntered(string obj)
        {
            scanQRMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}