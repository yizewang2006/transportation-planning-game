using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float sprintSpeedMultiplier = 2f;
    public float rotationSpeed = 2f;
    public float rotationSmoothness = 5f;

    private bool isRotating;
    private Vector3 initialRotation;
    private Vector3 initialPosition;
    private bool hasInitialRotation = false;

    private Quaternion targetRotation;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check for right mouse button down to start rotating
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            initialRotation = transform.eulerAngles;
            initialPosition = Input.mousePosition;
            hasInitialRotation = true;
        }

        // Check for right mouse button up to stop rotating
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        // Handle camera rotation
        if (isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = currentMousePosition.x - initialPosition.x;
            float deltaY = currentMousePosition.y - initialPosition.y;

            float newRotationX = initialRotation.x - deltaY * rotationSpeed;
            float newRotationY = initialRotation.y + deltaX * rotationSpeed;

            // Limit the vertical rotation between -90 and 90 degrees
            newRotationX = Mathf.Clamp(newRotationX, -90f, 90f);

            targetRotation = Quaternion.Euler(newRotationX, newRotationY, 0f);
        }

        // Smoothly rotate the camera towards the target rotation if initial rotation has been set
        if (hasInitialRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
        }

        // Handle camera movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float currentMovementSpeed = movementSpeed;
        if (isSprinting)
        {
            currentMovementSpeed *= sprintSpeedMultiplier;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * currentMovementSpeed;
        movement = transform.TransformDirection(movement);

        rb.velocity = movement;
    }
}
