using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class CarSpawnManager : MonoBehaviour
{
    public CarSpawner carStationPrefab;
    public float carSpawnInterval = 3;
    public Vector2 peopleIncreaseInterval;
    public Vector2 peopleIncreaseCount;
    public int carStationCount;
    [HorizontalLine]
    public Transform carStationsParent;
    public ObjectPooler carPooler;
    [HorizontalLine]
    [ReadOnly] public List<Node> startPathList;
    [ReadOnly] public List<Node> endPathList;

    public static CarSpawnManager Instance;
    NodeManager nodeManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        nodeManager = NodeManager.Instance;
        GenerateUniquePaths();
        GenerateCarSpawners();
    }

    void GenerateUniquePaths()
    {
        startPathList = new List<Node>(nodeManager.carStationNodes);
        endPathList = new List<Node>(nodeManager.endStationNodes);
        List<(Node StartNode, Node EndNode)> uniquePairs = startPathList
        .Zip(endPathList, (startNode, endNode) => (StartNode: startNode, EndNode: endNode))
        .Distinct()
        .ToList();

        startPathList = uniquePairs.Select(pair => pair.StartNode).ToList();
        endPathList = uniquePairs.Select(pair => pair.EndNode).ToList();
    }

    void GenerateCarSpawners()
    {
        for (int i = 0; i < carStationCount; i++)
        {
            if(i > startPathList.Count - 1) return;
            CarSpawner newSpawner = Instantiate(carStationPrefab, startPathList[i].transform.position, Quaternion.identity)
            .GetComponent<CarSpawner>();

            newSpawner.startNode = startPathList[i];
            newSpawner.endNode = endPathList[i];
            newSpawner.transform.SetParent(carStationsParent);
        }
    }

}
