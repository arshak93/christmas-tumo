using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private ConjureKitWrapper conjureKitWrapper;
    [SerializeField] private Button ornamentsButton;
    [SerializeField] private OrnamentMenu ornamentsMenu;
    [SerializeField] private GameObject scanQRMenu;
    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
        conjureKitWrapper.OnDomainEntered += OnDomainEntered;
        ornamentsButton.onClick.AddListener(OnOrnamentsButtonClick);
    }

    private void OnOrnamentsButtonClick()
    {
        ornamentsMenu.Show();
    }

    private void OnDomainEntered(string obj)
    {
        scanQRMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
