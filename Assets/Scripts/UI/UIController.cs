using Posemesh;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private ConjureKitWrapper conjureKitWrapper;
        [SerializeField] private Button ornamentsButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private GameObject scanQRMenu;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] Animator ornamentsMenuAnimator;
        [SerializeField] GameObject ornamentsMenu;
    
        void OnEnable()
        {
            ornamentsButton.onClick.AddListener(ShowOrnamentsMenu);
            cancelButton.onClick.AddListener(HideOrnamentsMenu);
        }

        void OnDisable()
        {
            ornamentsButton.onClick.RemoveListener(ShowOrnamentsMenu);
            cancelButton.onClick.RemoveListener(HideOrnamentsMenu);
        }
    
        private void Start()
        {
            conjureKitWrapper.OnDomainEntered += OnDomainEntered;
        }
    
        void ShowOrnamentsMenu()
        {
            ornamentsMenu.SetActive(true);
            if (!ornamentsMenuAnimator.GetBool("Show"))
            {
                ornamentsMenuAnimator.SetBool("Show", true);
            }
        }
    
        public void HideOrnamentsMenu()
        {
            ornamentsMenuAnimator.SetBool("Show", false);
        }

        private void OnDomainEntered(string obj)
        {
            scanQRMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
