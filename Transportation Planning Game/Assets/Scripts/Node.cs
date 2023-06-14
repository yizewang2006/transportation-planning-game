using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif

public class Node : MonoBehaviour
{
    public List<Node> connectedNodes = new List<Node>();
    public Color connectionColor;

    [Space(15)]
    public NodeType nodeType;
    public enum NodeType { Normal, Car, Bus };

#if UNITY_EDITOR
    static Node()
    {
        EditorApplication.delayCall += AssignRandomColor;
    }

    private void Reset()
    {
        AssignRandomColor();
    }

    private static void AssignRandomColor()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        List<Color> usedColors = new List<Color>();

        foreach (Node node in nodes)
        {
            if (node != null && node.connectionColor != Color.clear)
            {
                usedColors.Add(node.connectionColor);
            }
        }

        List<Node> selectedNodes = new List<Node>();

        foreach (GameObject selectedObject in Selection.gameObjects)
        {
            Node selectedNode = selectedObject.GetComponent<Node>();
            if (selectedNode != null)
            {
                selectedNodes.Add(selectedNode);
            }
        }

        foreach (Node selectedNode in selectedNodes)
        {
            Color newColor;
            do
            {
                newColor = GetRandomColor();
            }
            while (usedColors.Contains(newColor));

            usedColors.Add(newColor);
            selectedNode.connectionColor = newColor;
        }
    }
#endif

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
