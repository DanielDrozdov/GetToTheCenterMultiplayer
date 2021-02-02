using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedMenuPanelController : MonoBehaviour
{
    private enum SelectedPanel {
        Settings,
        Servers
    }

    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ServersPanel;
    [SerializeField] private GameObject SelectedMenuPanelBG;
    private SelectedPanel selectedPanel;
    private GameObject lastOpenedPanel;

    public void ClosePanel() {
        lastOpenedPanel.SetActive(false);
        SelectedMenuPanelBG.SetActive(false);
    }

    public void OpenSettingsPanel() {
        selectedPanel = SelectedPanel.Settings;
        OpenPanel();
    }

    public void OpenServersPanel() {
        selectedPanel = SelectedPanel.Servers;
        OpenPanel();
    }

    private void OpenPanel() {
        if(SelectedMenuPanelBG.activeSelf) {
            ServersPanel.SetActive(false);
            SettingsPanel.SetActive(false);
        } else {
            SelectedMenuPanelBG.SetActive(true);
        }

        if(selectedPanel == SelectedPanel.Servers) {
            ServersPanel.SetActive(true);
            lastOpenedPanel = ServersPanel;
        } else if(selectedPanel == SelectedPanel.Settings) {
            SettingsPanel.SetActive(true);
            lastOpenedPanel = SettingsPanel;
        }
    }
}
