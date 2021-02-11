using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private int oldInteger;

    public void UpdateCountDownPanel(float seconds) {
        if(seconds > 0) {
            float newInteger = Mathf.Floor(seconds);
            if(oldInteger != newInteger) {
                text.text = newInteger.ToString();
                oldInteger = (int)newInteger;
            }
        }
    }
}
