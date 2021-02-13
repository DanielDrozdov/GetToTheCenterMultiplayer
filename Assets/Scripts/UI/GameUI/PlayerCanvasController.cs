using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCanvasController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject startText;
    [SerializeField] private TextMeshProUGUI winRatingText;
    private int countDownOldInteger;

    public void UpdateCountDownPanel(float seconds) {
        if(seconds > 0) {
            float newInteger = Mathf.Floor(seconds);
            if(countDownOldInteger != newInteger) {
                countDownText.text = newInteger.ToString();
                countDownOldInteger = (int)newInteger;
            }
        }
    }

    public void PlayStartTextAnimation() {
        startText.SetActive(true);
    }

    public void PlayPlayerWinRatingTextAnimation(string _winRatingText) {
        winRatingText.gameObject.SetActive(true);
        winRatingText.text = _winRatingText;
    }
}
