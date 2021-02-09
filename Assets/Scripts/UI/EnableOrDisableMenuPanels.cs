using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOrDisableMenuPanels : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject selectedMenuPanel;
    private static EnableOrDisableMenuPanels Instance;

    private void Awake() {
        Instance = this;
    }

    private EnableOrDisableMenuPanels() { }

    public static EnableOrDisableMenuPanels GetInstance() {
        return Instance;
    }

    public void ActivateMenu() {
        menuPanel.SetActive(true);
    }

    public void DeactivateMenuPanel() {
        menuPanel.SetActive(false);
    }

    public void ActivateLobby() {
        lobbyPanel.SetActive(true);
    }

    public void DeactivateLobby() {
        lobbyPanel.SetActive(false);
    }

    public void DeactivateServersAnsSettingsPanels() {
        selectedMenuPanel.SetActive(false);
    }

    public void DeactivateMainPanelsForLobby() {
        ActivateLobby();
        DeactivateServersAnsSettingsPanels();
        DeactivateMenuPanel();
    }
}
