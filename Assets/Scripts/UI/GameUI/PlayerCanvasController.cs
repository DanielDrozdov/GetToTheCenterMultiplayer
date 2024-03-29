﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerCanvasController : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject startText;
    [SerializeField] private TextMeshProUGUI winRatingText;
    [SerializeField] private TapController tapController;
    [SerializeField] private GameObject roundResultPanelController;
    [SerializeField] private GameObject leaveRoomButton;
    private int countDownOldInteger;

    private void Awake() {
        SceneNetworkController.OnGameEnd += ActivateRoundResultPanel;
        SceneNetworkController.OnGameStarted += ActivateTapController;
        SceneNetworkController.OnGameStarted += PlayStartTextAnimation;
    }

    public bool UpdateCountDownPanel(float seconds) {
        if(seconds > 0) {
            float newInteger = Mathf.Floor(seconds);
            if(countDownOldInteger != newInteger) {
                countDownText.text = newInteger.ToString();
                countDownOldInteger = (int)newInteger;
                return true;
            }
        }
        return false;
    }

    public void PlayStartTextAnimation() {
        startText.SetActive(true);
    }

    public void PlayPlayerWinRatingTextAnimation(string _winRatingText) {
        winRatingText.gameObject.SetActive(true);
        winRatingText.text = _winRatingText;
    }

    private void ActivateRoundResultPanel() {
        DeactivatePlayerGameUI();
        roundResultPanelController.SetActive(true);
    }

    private void DeactivatePlayerGameUI() {
        leaveRoomButton.SetActive(false);
        countDownText.gameObject.SetActive(false);
    }

    private void ActivateTapController() {
        tapController.IfCanMove = true;
    }
}
