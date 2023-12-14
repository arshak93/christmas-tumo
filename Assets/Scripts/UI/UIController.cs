using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button ornamentsButton;
    [SerializeField] private OrnamentMenu ornamentsMenu;

    private void Start()
    {
        ornamentsButton.onClick.AddListener(OnOrnamentsButtonClick);
    }

    private void OnOrnamentsButtonClick()
    {
        ornamentsMenu.Show();
    }
}
