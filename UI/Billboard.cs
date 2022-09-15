using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    Vector3 defaultScale;

    void Start()
    {
        cam = Camera.main.transform;
        defaultScale = transform.localScale;
    }

    void LateUpdate()
    {
        transform.localScale = new Vector3(cam.transform.position.y / 1800, cam.transform.position.y / 1800, 0.00f);
        transform.LookAt(transform.position + cam.forward);
    }
}
