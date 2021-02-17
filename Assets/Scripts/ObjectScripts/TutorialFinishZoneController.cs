using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFinishZoneController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            StartCoroutine(OneSecondDelayAndLoadMenuCoroutine());
        }
    }

    private IEnumerator OneSecondDelayAndLoadMenuCoroutine() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
}
