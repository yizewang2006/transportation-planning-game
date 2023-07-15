using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class PrefabUnpacker : EditorWindow
{
    [MenuItem("Tools/Unpack Child Prefabs")]
    private static void UnpackChildObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject parentObject in selectedObjects)
        {
            UnpackChildPrefabsRecursively(parentObject.transform);
        }
    }

    private static void UnpackChildPrefabsRecursively(Transform parentTransform)
    {
        int childCount = parentTransform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform childTransform = parentTransform.GetChild(i);
            GameObject childObject = childTransform.gameObject;

            // Check if the child object is a prefab instance
            if (PrefabUtility.IsPartOfAnyPrefab(childObject))
            {
                // Unpack the child prefab instance
                PrefabUtility.UnpackPrefabInstance(childObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }

            // Recursively unpack child prefabs of the child object
            UnpackChildPrefabsRecursively(childTransform);
        }
    }
}
#endif