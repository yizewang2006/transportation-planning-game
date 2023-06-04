using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusMotion : MonoBehaviour
{
    // Variables
    public float currentVelocity;
    public float normalVelocity;
    public float acceleration;

    public bool busStop;

    TrackFollow trackFollowScript;
    [SerializeField] private Collider busStopCollider;

    private void Awake()
    {
        trackFollowScript = GetComponent<TrackFollow>();
    }

    private void Update()
    {
        trackFollowScript.m_Speed = currentVelocity;
        DetectObject(10f);
    }

    void StopBus()
    {
        if (busStop)
        {
            busStopCollider.enabled = false; // Disable the bus stop collider to prevent further detections
            Accelerate(0); // Brake the bus to 0
        }
    }

    void ReturnToNormalVelocity()
    {
        busStopCollider.enabled = true; // Enable the bus stop collider when returning to normal velocity
        Accelerate(normalVelocity);
    }

    // We want the bus to have smooth motion
    void Accelerate(float targetVelocity)
    {
        if (currentVelocity < targetVelocity)
        {
            currentVelocity += acceleration * Time.deltaTime;
        }
        else if (currentVelocity > targetVelocity)
        {
            currentVelocity -= acceleration * Time.deltaTime;
        }
    }

    // Detect if there's any object before the bus within a certain radius
    void DetectObject(float maxDistance)
    {
        // Define the origin and direction of the raycast
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        // Create a raycast hit variable to store the information about the hit
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance))
        {
            // If 'hit' is a bus stop signal
            if (hit.collider.gameObject.CompareTag("Bus Stop"))
            {
                busStopCollider = hit.collider;
                StartCoroutine(StopAtStation());
            }
        }
    }

    IEnumerator StopAtStation()
    {
        StopBus();
        if (Mathf.Round(currentVelocity) == 0) busStopCollider.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        ReturnToNormalVelocity(); // <<< What?
    }
}