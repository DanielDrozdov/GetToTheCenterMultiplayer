using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVController : MonoBehaviour
{
    private Camera playerCamera;
    private float normalFOV = 60f;
    private float maxFOV = 65f;
    private float FOVSpeed = 10f;
    private float currentFOV;

    private bool IsMove;
    private float FOVTimeActive = 0.4f;
    private float remainingFOVTimeActive;

    private void Awake() {
        playerCamera = GetComponent<Camera>();
        currentFOV = normalFOV;
    }

    public void ActivateFov() {
        IsMove = true;
        remainingFOVTimeActive = FOVTimeActive;
    }

    private void Update() {
        CountDownFastFOVDeactivation();
        CheckFastFOVState();
    }

    private void CountDownFastFOVDeactivation() {
        remainingFOVTimeActive -= Time.deltaTime;
        if(remainingFOVTimeActive <= 0) {
            IsMove = false;
        }
    }

    private void CheckFastFOVState() {
        if(IsMove) {
            currentFOV += FOVSpeed * Time.deltaTime;
        } else {
            currentFOV -= FOVSpeed * Time.deltaTime;
        }
        currentFOV = Mathf.Clamp(currentFOV, normalFOV, maxFOV);
        playerCamera.fieldOfView = currentFOV;
    }
}
