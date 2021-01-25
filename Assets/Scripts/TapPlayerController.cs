using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapPlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector3(0, 0, -0.5f);
        }
    }
}
