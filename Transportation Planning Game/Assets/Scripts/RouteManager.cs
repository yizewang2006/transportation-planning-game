using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public List<Node> terminals = new List<Node>();
    public int pathCount;
    public Transform routeHolder;

    [Space(20)]
    public Vector2 peopleRange;
    public int peoplePerCar;
    public float carSpawnInterval;
    public List<GameObject> carPrefab = new List<GameObject>();
    public List<PathFinder> spawnedRoutes = new List<PathFinder>();

    

    void Start()
    {
        GenerateRandomPaths();
    }

    void GenerateRandomPaths()
    {
        if (terminals.Count < 2)
        {
            Debug.LogError("Not enough spawn nodes available to generate paths.");
            return;
        }
        spawnedRoutes.Clear();

        List<Node> availableStartNodes = new List<Node>();
        List<Node> availableEndNodes = new List<Node>();

        for (int i = 0; i < terminals.Count; i++)
        {
            for (int j = 0; j < terminals.Count; j++)
            {
                if (i != j)
                {
                    availableStartNodes.Add(terminals[i]);
                    availableEndNodes.Add(terminals[j]);
                }
            }
        }

        if (pathCount > availableStartNodes.Count)
        {
            Debug.LogWarning("Path count exceeds the number of possible unique paths.");
            pathCount = availableStartNodes.Count;
        }

        for (int i = 0; i < pathCount; i++)
        {
            if (availableStartNodes.Count == 0 || availableEndNodes.Count == 0)
            {
                Debug.LogWarning("Not enough available start or end nodes to generate all paths.");
                break;
            }

            int randomPath = Random.Range(0, availableStartNodes.Count);
            
            PathFinder path = new GameObject("Routes " + (i+1)).AddComponent<PathFinder>();
            path.start = availableStartNodes[randomPath];
            path.end = availableEndNodes[randomPath];
            path.debugColor = Node.GetRandomColor();
            PathFinder.AddAllNodes(path);

            path.transform.SetParent(routeHolder);

            availableStartNodes.RemoveAt(randomPath);
            availableEndNodes.RemoveAt(randomPath);
            spawnedRoutes.Add(path);

            CarSpawner newSpawner = path.AddComponent<CarSpawner>();
            newSpawner.path = path;
            newSpawner.maxPeople = Random.Range((int)peopleRange.x, (int)peopleRange.y+1);
            newSpawner.peoplePerCar = peoplePerCar;
            newSpawner.spawnInterval = carSpawnInterval;
            newSpawner.carPrefab = new List<GameObject>(carPrefab);
        }
    }
}
