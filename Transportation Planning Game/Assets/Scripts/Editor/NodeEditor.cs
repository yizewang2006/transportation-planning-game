using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    private bool isConnecting;
    private Node startNode;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Node node = (Node)target;

        GUILayout.Space(10);

        if (!isConnecting)
        {
            if (GUILayout.Button("Start Connecting"))
            {
                isConnecting = true;
                startNode = node;
                Debug.Log("Start connecting from Node: " + startNode.name);
            }
        }
        else
        {
            GUILayout.Label("Click on other nodes to connect to:");
            if (GUILayout.Button("Cancel"))
            {
                isConnecting = false;
                Debug.Log("Connection canceled");
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Clear List"))
        {
            node.connectedNodes.Clear();
            Debug.Log("Cleared connected nodes list for Node: " + node.name);
        }
    }

    private void OnSceneGUI()
    {
        if (isConnecting)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (e.type == EventType.MouseDown && e.button == 0 && Physics.Raycast(ray, out hit))
            {
                Node endNode = hit.collider.GetComponent<Node>();
                if (endNode != null && endNode != startNode && !startNode.connectedNodes.Contains(endNode))
                {
                    startNode.connectedNodes.Add(endNode);
                    Debug.Log("Connected Node: " + endNode.name);
                }
            }

            if (e.type == EventType.KeyUp && e.keyCode == KeyCode.Escape)
            {
                isConnecting = false;
                Debug.Log("Connection canceled");
            }
        }
    }
}
