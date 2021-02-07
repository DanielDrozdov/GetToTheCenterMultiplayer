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
    [SerializeField] private TextMeshProUGUI warningText;

    private string warning_FailedNameRoom = "You did not fill in the name of the room!";
    private string warning_NameRoomIsUse = "This room name is already in use";

    public void OnClick_CreateRoomButton() {
        ActivateCreateRoomPanel();
        selectedMenuPanelController.ClosePanels();
    }

    public void OnClick_AcceptRoomSettingsButton() {
        string roomName = roomNameInput.text;
        if(string.IsNullOrWhiteSpace(roomName)) {
            ActivateWarningText(warning_FailedNameRoom);
        }
        foreach(RoomInfo room in ServersPanelController.GetCachedRoomDictionary().Values) {
            if(room.Name == roomName) {
                ActivateWarningText(warning_NameRoomIsUse);
            }
        }

        networkMenuController.CreateRoom(roomNameInput.text, dropdownMapChoice.captionText.text);
        
    }

    private void ActivateCreateRoomPanel() {
        gameObject.SetActive(true);
    }

    private void ActivateWarningText(string warningMessage) {
        warningText.enabled = true;
        warningText.text = warningMessage;
        return;
    }
}
