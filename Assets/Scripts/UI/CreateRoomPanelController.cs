using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateRoomPanelController : MonoBehaviour {

    [SerializeField] private SelectedMenuPanelController selectedMenuPanelController;
    [SerializeField] private NetworkMenuController networkMenuController;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_Dropdown dropdownMapChoice;
    [SerializeField] private TMP_Dropdown dropdownDifficultyChoice;
    [SerializeField] private TextMeshProUGUI warningText;

    private string warning_FailedNameRoom = "You did not fill in the name of the room!";
    private string warning_NameRoomIsUse = "This room name is already in use";

    private EnableOrDisableMenuPanels MenuPanelsDisableAndEnableControllerInstance;

    public void OnClick_CreateRoomButton() {
        MenuPanelsDisableAndEnableControllerInstance = EnableOrDisableMenuPanels.GetInstance();
        ActivateCreateRoomPanel();
        selectedMenuPanelController.ClosePanels();
    }

    public void OnClick_AcceptRoomSettingsButton() {
        string roomName = roomNameInput.text;
        if(string.IsNullOrWhiteSpace(roomName)) {
            ActivateWarningText(warning_FailedNameRoom);
            return;
        }
        foreach(RoomInfo room in ServersPanelController.GetCachedRoomDictionary().Values) {
            if(room.Name == roomName) {
                ActivateWarningText(warning_NameRoomIsUse);
                return;
            }
        }
        networkMenuController.CreateRoom(roomNameInput.text, dropdownMapChoice.captionText.text, dropdownDifficultyChoice.captionText.text);
        gameObject.SetActive(false);
        MenuPanelsDisableAndEnableControllerInstance.ActivateLobby();
    }

    public void OnClick_CancelCreateRoom() {
        MenuPanelsDisableAndEnableControllerInstance.ActivateMenu();
        gameObject.SetActive(false);
    }

    private void ActivateCreateRoomPanel() {
        MenuPanelsDisableAndEnableControllerInstance.DeactivateMenuPanel();
        gameObject.SetActive(true);
    }

    private void ActivateWarningText(string warningMessage) {
        warningText.enabled = true;
        warningText.text = warningMessage;
    }

    private void OnDisable() {
        roomNameInput.text = "";
    }
}
