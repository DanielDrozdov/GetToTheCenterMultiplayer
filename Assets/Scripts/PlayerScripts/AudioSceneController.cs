using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioSceneController : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioClip playerWinAudioClip;
    [SerializeField] private AudioClip endGameAudioClip;
    [SerializeField] private AudioClip gongAudioClip;
    [SerializeField] private AudioClip tickAudioClip;
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource secondMainAudioSource;
    private static AudioSceneController Instance;

    private void Awake() {
        Instance = this;
    }

    public static AudioSceneController GetInstance() {
        return Instance;
    }

    public void PlayGongAudio() {
        PlayAudio(secondMainAudioSource,gongAudioClip);
    }

    public void PlayTickAudio() {
        PlayAudio(mainAudioSource,tickAudioClip);
    }

    public void PlayGameEndAudio() {
        PlayAudio(mainAudioSource, endGameAudioClip);
    }

    public void PlayWinAudio() {
        PlayAudio(secondMainAudioSource,playerWinAudioClip);
    }

    private void PlayAudio(AudioSource audioSource,AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
