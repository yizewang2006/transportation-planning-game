using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class PathFinder : MonoBehaviour
{
    public List<Intersection> intersections = new List<Intersection>();
    public List<Intersection> bestPath = new List<Intersection>();
    public Intersection start;
    public Intersection end;

    void Update()
    {
        if(start != end && start != null & end != null && intersections.Count >= 2)
            bestPath = CalculateBestPath(start, end);
        else
            bestPath.Clear();
    }

    void OnDrawGizmos()
    {
        foreach (Intersection inter in bestPath)
        {
            if (bestPath.Count < 2)
                return;

            for (int i = 0; i < bestPath.Count - 1; i++)
            {
                GameObject objA = bestPath[i].gameObject;
                GameObject objB = bestPath[i + 1].gameObject;

                if (objA != null && objB != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(objA.transform.position, objB.transform.position);
                }
            }
        }
    }

    public List<Intersection> CalculateBestPath(Intersection starting, Intersection ending)
    {
        foreach (var i in intersections)
            i.searched = false;

        Intersection currentSearched = starting;                            // Stores the current node being searched
        List<Intersection> bestPath = new List<Intersection>();            // The list of the best path from start to end
        bestPath.Add(starting);

        while (true)
        {
            Intersection nearestNextNode = GetNearestNextNode(currentSearched, ending);

            currentSearched.searched = true;

            bestPath.Add(nearestNextNode);
            currentSearched = nearestNextNode;

            if (nearestNextNode == ending) break;
        }

        return bestPath;
    }

    public Intersection GetNearestNextNode(Intersection searchFrom, Intersection ending)
    {
        Intersection nearestNode = null;
        float nearestDistance = float.MaxValue;

        foreach (Intersection obj in searchFrom.connectedNodes)
        {
            if (obj.searched) continue;
            float distance = Vector3.Distance(obj.transform.position, ending.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestNode = obj;
            }
        }

        return nearestNode;
    }
}
