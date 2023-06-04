using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(PathFinder))]
public class SmoothPathGenerator : MonoBehaviour
{
    public CinemachineSmoothPath track;
    public float smoothing = 3;

    PathFinder pathFinder;

    void Update()
    {
        if(pathFinder == null) pathFinder = GetComponent<PathFinder>();
        if (track != null && pathFinder != null) GenerateSmoothPath(pathFinder);
    }

    public void GenerateSmoothPath(PathFinder path)
    {
        List<CinemachineSmoothPath.Waypoint> waypoints = new List<CinemachineSmoothPath.Waypoint>();
        CinemachineSmoothPath.Waypoint newPoint = new CinemachineSmoothPath.Waypoint();

        if (path.bestPath.Count > 0)
        {
            for (int i = 0; i < path.bestPath.Count; i++)
            {
                if (i == 0 || i == path.bestPath.Count - 1)
                {
                    newPoint.position = path.bestPath[i].transform.position;
                    waypoints.Add(newPoint);
                }
                else if(i + 1 < path.bestPath.Count)
                {
                    Transform targetNode = path.bestPath[i].transform;
                    Transform prevNode = path.bestPath[i - 1].transform;
                    Vector3 midPoint = (targetNode.position + prevNode.position) / 2f;
                    Vector3 direction = (targetNode.position - prevNode.position).normalized;
                    newPoint.position = (midPoint + (targetNode.position - prevNode.position) / 2) + (direction * -smoothing);
                    waypoints.Add(newPoint);

                    targetNode = path.bestPath[i].transform;
                    prevNode = path.bestPath[i + 1].transform;
                    midPoint = (targetNode.position + prevNode.position) / 2f;
                    direction = (targetNode.position - prevNode.position).normalized;
                    newPoint.position = (midPoint + (targetNode.position - prevNode.position) / 2) + (direction * -smoothing);
                    waypoints.Add(newPoint);
                }

            }
        }
        track.m_Waypoints = waypoints.ToArray();
    }
}
