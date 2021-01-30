using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour, IPointerDownHandler
{
    public GameObject GameTouchPanel;
    private GameObject tutorialOtherComponents;
    private Image tutorialPanel;

    private void Awake() {
        tutorialPanel = GetComponent<Image>();
        tutorialOtherComponents = transform.Find("TutoiralPanelOtherComponents").gameObject;
        StartCoroutine(TwoSecondsDelayCoroutine());
    }

    public void OnPointerDown(PointerEventData eventData) {
        GameTouchPanel.SetActive(true);
        tutorialOtherComponents.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator TwoSecondsDelayCoroutine() {
        float delayTime = 1f;
        while(true) {
            delayTime -= Time.deltaTime;
            if(delayTime <= 0) {
                tutorialPanel.enabled = true;
                tutorialOtherComponents.SetActive(true);
                yield break;
            }
            yield return null;
        }
    }
}
