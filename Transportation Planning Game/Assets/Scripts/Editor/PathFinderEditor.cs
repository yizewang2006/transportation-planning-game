using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathFinder))]
public class PathFinderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathFinder pathFinder = (PathFinder)target;

        GUILayout.Space(10f);

        if (GUILayout.Button("Add All Nodes"))
        {
            PathFinder.AddAllNodes(pathFinder);
        }

        if (GUILayout.Button("Clear Node List"))
        {
            ClearNodeList(pathFinder);
        }
    }



    private void ClearNodeList(PathFinder pathFinder)
    {
        pathFinder.nodes.Clear();
        Debug.Log("Cleared the node list.");
    }
}
