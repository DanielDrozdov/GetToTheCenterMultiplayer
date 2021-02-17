using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioSceneController : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioClip playerWinAudioClip;
    [SerializeField] private AudioClip endGameAudioClip;
    [SerializeField] private AudioClip tapAudioClip;
    [SerializeField] private AudioClip gongAudioClip;
    [SerializeField] private AudioClip tickAudioClip;
    private static AudioSceneController Instance;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if(!photonView.IsMine) {
            gameObject.SetActive(false);
        } else {
            Instance = this;
        }
    }

    public static AudioSceneController GetInstance() {
        return Instance;
    }

    public void PlayGongAudio() {
        PlayAudio(gongAudioClip);
    }

    public void PlayTickAudio() {
        PlayAudio(tickAudioClip);
    }

    public void PlayGameEndAudio() {
        PlayAudio(endGameAudioClip);
    }

    public void PlayTapAudio() {
        PlayAudio(tapAudioClip);
    }

    public void PlayWinAudio() {
        PlayAudio(playerWinAudioClip);
    }

    private void PlayAudio(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
