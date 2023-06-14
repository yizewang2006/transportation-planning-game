using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public List<PathFinder> pathNodes = new List<PathFinder>(); // List of path nodes in order
    public float moveSpeed = 2;
    public float turnSmoothness = 5f; // Controls the smoothness of the turns
    private Quaternion targetRotation; // Target rotation for smooth turning
    public bool loop;
    int currentNodeIndex = 0;
    int currentPath;

    void Start()
    {
        currentPath = 0;
        ResetPosition();
        MoveToNextNode();
    }

    public void ResetPosition()
    {
        if (pathNodes != null && pathNodes[currentPath].bestPath.Count > 0)
            transform.position = pathNodes[currentPath].bestPath[0].transform.position;
    }

    void Update()
    {
        // Check if the car has reached the current node
        if (Mathf.Abs(transform.position.x - pathNodes[currentPath].bestPath[currentNodeIndex].transform.position.x) < 0.1f &&
            Mathf.Abs(transform.position.z - pathNodes[currentPath].bestPath[currentNodeIndex].transform.position.z) < 0.1f)
        {
            MoveToNextNode();
        }

        if(!loop && Mathf.Abs(transform.position.x - pathNodes[0].end.transform.position.x) < 0.1f && 
        Mathf.Abs(transform.position.z - pathNodes[0].end.transform.position.z) < 0.1f) Destroy(gameObject);
    }

    void MoveToNextNode()
    {
        if (currentNodeIndex < pathNodes[currentPath].bestPath.Count - 1)
        {
            currentNodeIndex++;
            StartCoroutine(MoveTowardsNode(pathNodes[currentPath].bestPath[currentNodeIndex].transform.position));
        }
        else
        {
            // Reached the end of the path
            if (loop && pathNodes.Count > 1)
            {
                currentNodeIndex = 0;
                currentPath++;
                if (currentPath >= pathNodes.Count) currentPath = 0;
            }
        }
    }

    IEnumerator MoveTowardsNode(Vector3 targetPosition)
    {
        Quaternion targetRotation = transform.rotation;

        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.1f || Mathf.Abs(transform.position.z - targetPosition.z) > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
            targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSmoothness * Time.deltaTime);
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            transform.rotation = targetRotation;

            Vector3 targetPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

    }
}
