using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour
{
    Transform cam;
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction from the quad to the camera
        Vector3 cameraDirection = cam.position - transform.position;

        // Calculate the rotation needed to face the camera
        Quaternion targetRotation = Quaternion.LookRotation(-cameraDirection);

        // Apply the rotation to the quad
        transform.rotation = targetRotation;
    }
}
