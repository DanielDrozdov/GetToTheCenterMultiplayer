using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject actionPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject nicknameInputPanel;

    private void Awake() {
        if(!PlayerPrefs.HasKey(DataStorage.IsFirstEnterInGamePlayerPrefs)) {
            ActivateTutorialPanel();
            PlayerPrefs.SetInt(DataStorage.IsFirstEnterInGamePlayerPrefs, 1);
        } else if(!PlayerPrefs.HasKey(DataStorage.NicknamePlayerPrefs)) {
            ActivateNicknameInputPanel();
        }
    }

    public void ActivateTutorialPanel() {
        actionPanel.SetActive(true);
        tutorialPanel.SetActive(true);
    }

    public void DisactivateTutorialPanel() {
        tutorialPanel.SetActive(false);
        nicknameInputPanel.SetActive(true);
    }

    public void ActivateNicknameInputPanel() {
        actionPanel.SetActive(true);
        nicknameInputPanel.SetActive(true);
    }

    public void OnAgreeTutorial() {
        SceneManager.LoadScene(1);
    }
}
