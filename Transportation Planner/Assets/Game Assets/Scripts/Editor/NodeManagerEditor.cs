using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeManager))]
public class NodeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NodeManager nodeManager = (NodeManager)target;

        GUILayout.Space(10f);

        if (GUILayout.Button("Add All Nodes"))
        {
            NodeManager.AddAllNodes(nodeManager);
        }

        if (GUILayout.Button("Clear Node List"))
        {
            ClearNodeList(nodeManager);
        }
    }



    private void ClearNodeList(NodeManager nodeManager)
    {
        nodeManager.nodes.Clear();
        Debug.Log("Cleared the node list.");
    }
}
