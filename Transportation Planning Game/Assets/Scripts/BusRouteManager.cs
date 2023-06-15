using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusRouteManager : MonoBehaviour
{
    public float mouseRayCheck;

    RaycastHit hit;
    public Transform busLineCircle;
    public LineRenderer busLine;
    public float busLineWidth;
    public Node currentSelectedNode;
    public Node start;
    public Node end;
    public bool complete;

    void Update()
    {
        BusPathCreator();
        BusPathDrawer();
    }

    void BusPathCreator()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Collider[] colliders = Physics.OverlapSphere(hit.point, mouseRayCheck);
                float closestDistance = float.MaxValue;

                foreach (Collider collider in colliders)
                {
                    Node node = collider.GetComponent<Node>();

                    if (node != null && node.nodeType == Node.NodeType.Bus)
                    {
                        float distance = Vector3.Distance(hit.point, collider.ClosestPoint(hit.point));

                        if (distance < closestDistance)
                        {
                            currentSelectedNode = node;
                            closestDistance = distance;
                        }
                    }
                }

                if(currentSelectedNode != null)
                {
                    if(start == null) 
                    {
                        start = currentSelectedNode;
                        Instantiate(busLineCircle, start.transform.position, Quaternion.identity);
                    }
                    else if(end == null)
                    {
                        Instantiate(busLineCircle, start.transform.position, Quaternion.identity);
                        end = currentSelectedNode;
                        if(start != null && end != null) complete = true;
                    }
                }
            }
        }

    }

    void BusPathDrawer()
    {
        if(complete)
        {
            busLine.widthMultiplier = busLineWidth;
            PathFinder newPath = new PathFinder();
            PathFinder.AddAllNodes(newPath);
            List<Node> bestPath = newPath.CalculateBestPath(start, end, newPath.nodes);
            busLine.positionCount = bestPath.Count;
            for (int i = 0; i < bestPath.Count; i++)
            {
                busLine.SetPosition(i, bestPath[i].transform.position);
            }
        }
    }
}
