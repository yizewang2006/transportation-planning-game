using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeMovement : MonoBehaviour
{
    public Rigidbody cam;
    public float movementSpeed = 500;
    public float sprintSpeed = 700;
    public float rotationSpeed = 300;

    [Space(15)]
    public bool invertX;
    public bool invertY;

    float rotationX = 0f;
    float rotationY = 0f;

    float speed;

    void Start()
    {
        rotationX = cam.rotation.eulerAngles.x;
        rotationY = cam.rotation.eulerAngles.y;
    }
    void Update()
    {
        speed = Input.GetKey(KeyCode.LeftShift)? sprintSpeed : movementSpeed;
        // Camera Movement
        float translationX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float translationZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 movement = (cam.transform.forward * translationZ) + (cam.transform.right * translationX);
        cam.velocity = movement;

        // Camera Rotation
        if (Input.GetMouseButton(2))
        {
            rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime * (invertY? -1 : 1);
            rotationY -= Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime * (invertX? -1 : 1);

            cam.rotation = Quaternion.Euler(rotationX, transform.eulerAngles.y + rotationY, 0f);
        }


    }
}
