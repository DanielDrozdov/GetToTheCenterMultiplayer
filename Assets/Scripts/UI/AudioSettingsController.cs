using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] private Toggle audioToogle;
    [SerializeField] private TextMeshProUGUI audioText;

    private void Awake() {
        audioToogle.onValueChanged.AddListener(delegate {
            ChangeAudioSettings(audioToogle);
        });
        if(PlayerPrefs.HasKey(DataStorage.AudioStatePlayerPrefs)) {
            int boolInt = PlayerPrefs.GetInt(DataStorage.AudioStatePlayerPrefs);
            if(boolInt == 1) {
                audioToogle.isOn = true;
                audioText.text = "Audio On:";
            } else {
                audioToogle.isOn = false;
                audioText.text = "Audio Off:";
            }
        } else {
            audioToogle.isOn = true;
            audioText.text = "Audio On:";
            PlayerPrefs.SetInt(DataStorage.AudioStatePlayerPrefs, 1);
        }
    }

    private void ChangeAudioSettings(Toggle audioToogle) {
        if(audioToogle.isOn) {
            audioText.text = "Audio On:";
            PlayerPrefs.SetInt(DataStorage.AudioStatePlayerPrefs,1);
        } else {
            audioText.text = "Audio Off:";
            PlayerPrefs.SetInt(DataStorage.AudioStatePlayerPrefs, 0);
        }
    }
}
