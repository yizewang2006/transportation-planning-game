using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PathFinder : MonoBehaviour
{
    public List<Node> nodes = new List<Node>();
    public List<Node> bestPath = new List<Node>();
    public Node start;
    public Node end;
    public Color debugColor = Color.yellow;

    void Update()
    {
        if (!start.Equals(end) && start != null && end != null && nodes.Count >= 2)
            bestPath = CalculateBestPath(start, end);
        else
            bestPath.Clear();
    }

    void OnDrawGizmos()
    {
        if (bestPath.Count < 2)
            return;

        for (int i = 0; i < bestPath.Count - 1; i++)
        {
            Node currentNode = bestPath[i];
            Node nextNode = bestPath[i + 1];

            if (currentNode != null && nextNode != null)
            {
                Gizmos.color = debugColor;
                Gizmos.DrawLine(currentNode.transform.position, nextNode.transform.position);
            }
        }
    }

    public static void AddAllNodes(PathFinder pathFinder)
    {
        pathFinder.nodes.Clear();

        Node[] nodes = FindObjectsOfType<Node>();

        foreach (Node node in nodes)
        {
            if (!pathFinder.nodes.Contains(node))
            {
                pathFinder.nodes.Add(node);
            }
        }

    }

    // Dijkstra's algorithm
    // https://youtu.be/pVfj6mxhdMw - Explanation
    public List<Node> CalculateBestPath(Node starting, Node ending)
    {
        Dictionary<Node, float> distances = new Dictionary<Node, float>(); // Stores the shortest distance from the starting node to each node
        Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>(); // Stores the previous node in the best path
        List<Node> unvisitedNodes = new List<Node>(); // Stores the nodes that haven't been visited yet

        // Initialization
        foreach (var node in nodes)
        {
            distances[node] = float.MaxValue;
            previousNodes[node] = null;
            unvisitedNodes.Add(node);
        }

        distances[starting] = 0;

        while (unvisitedNodes.Count > 0)
        {
            // Find the unvisited node with the smallest distance
            Node currentNode = null;
            float smallestDistance = float.MaxValue;
            foreach (var node in unvisitedNodes)
            {
                if (distances[node] < smallestDistance)
                {

                    smallestDistance = distances[node];
                    currentNode = node;
                }
            }

            if (currentNode == null)
                break;

            unvisitedNodes.Remove(currentNode);

            // Check if the current node is the ending node
            if (currentNode == ending)
                break;

            // Calculate the tentative distance for each connected node
            foreach (var neighbor in currentNode.connectedNodes)
            {
                float distance = Vector3.Distance(currentNode.transform.position, neighbor.transform.position);
                float tentativeDistance = distances[currentNode] + distance;

                if (tentativeDistance < distances[neighbor])
                {
                    distances[neighbor] = tentativeDistance;
                    previousNodes[neighbor] = currentNode;
                }
            }
        }

        // Build the best path by backtracking from the ending node
        List<Node> bestPath = new List<Node>();
        Node current = ending;
        while (current != null)
        {
            bestPath.Insert(0, current);
            current = previousNodes[current];
        }       
        return bestPath;
    }
}
