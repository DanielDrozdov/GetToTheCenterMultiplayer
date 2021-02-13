using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimationActionsController : MonoBehaviour
{
    public void OffTextAnimation() {
        PlayerCanvasNetworkController.OnWinnerRatingEnd();
        gameObject.SetActive(false);
    }
}
