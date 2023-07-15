using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public NodeType nodeType;
    public enum NodeType { Normal, CarStation, BusStation, EndStation };

    [Space(20)]
    public List<Node> connectedNodes;
    public Color connectionColor;

    void Update()
    {
        connectedNodes.RemoveAll(item => item == null || !item.gameObject.activeSelf);
    }

    public void Connect(Node newNode) { if (!IsConnected(newNode)) connectedNodes.Add(newNode); }
    public bool IsConnected(Node node) => connectedNodes.Contains(node);


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = connectionColor;
        foreach (Node connectedNode in connectedNodes)
        {
            if (connectedNode != null)
            {
                Gizmos.DrawLine(transform.position, connectedNode.transform.position);
            }
        }
    }

    public static Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        return new Color(r, g, b);
    }
}
