using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BusSpawner : MonoBehaviour
{
    ObjectPooler busPool;
    BusSpawnManager spawnManager;
    public Node startNode;
    public Node endNode;
    [ReadOnly] public CarSpawner nearestCarSpawner;
    [ReadOnly] public Node nearestEndNode;
    [HorizontalLine]
    public float checkRadius;
    public int peoplePerBus;
    [ReadOnly] public int currBuses;
    [ReadOnly] public int maxBuses;
    public Vector2 maxBusesRange;

    void Start()
    {
        spawnManager = BusSpawnManager.Instance;
        busPool = spawnManager.busPooler;

        maxBuses = Random.Range((int)maxBusesRange.x, (int)maxBusesRange.y + 1);
        InvokeRepeating("SpawnBus", 0, spawnManager.busSpawnInterval);
    }

    void Update()
    {
        if(nearestCarSpawner == null && nearestEndNode == null) CheckForAvailableSpawners();
        if (currBuses >= maxBuses) currBuses = maxBuses;
    }

    void CheckForAvailableSpawners()
    {
        if(startNode == null || endNode == null) return;
        Collider[] nearStartColliders = Physics.OverlapSphere(startNode.transform.position, checkRadius);
        Collider[] nearEndColliders = Physics.OverlapSphere(endNode.transform.position, checkRadius);
        float closestDistSpawner = Mathf.Infinity;
        float closestDistEnd = Mathf.Infinity;

        foreach (Collider collider in nearStartColliders)
        {
            CarSpawner carSpawner = collider.GetComponent<CarSpawner>();
            
            if (carSpawner != null)
            {
                float distance = Vector3.Distance(startNode.transform.position, collider.transform.position);

                if (distance < closestDistSpawner)
                {
                    closestDistSpawner = distance;
                    nearestCarSpawner = collider.GetComponent<CarSpawner>();
                }
            }
        }

        foreach (Collider collider in nearEndColliders)
        {
            Node endStationNode = collider.GetComponent<Node>();
            if (endStationNode != null && endStationNode.nodeType == Node.NodeType.EndStation)
            {
                float distance = Vector3.Distance(endStationNode.transform.position, collider.transform.position);

                if (distance < closestDistEnd)
                {
                    closestDistEnd = distance;
                    nearestEndNode = collider.GetComponent<Node>();
                }
            }
        }
    }

    void SpawnBus()
    {
        PathFinder pathFinder = GetComponent<PathFinder>();
        pathFinder.start = startNode;
        pathFinder.end = endNode;
        
        if (nearestCarSpawner == null || nearestEndNode == null) return;
        if (currBuses >= maxBuses) return;
        if (nearestCarSpawner.currPeople <= 0) return;

        if (nearestCarSpawner.currPeople >= peoplePerBus) nearestCarSpawner.currPeople -= peoplePerBus;
        else nearestCarSpawner.currPeople -= nearestCarSpawner.currPeople;
        
        CarDriver newBus = busPool.GrabFromPool(startNode.transform.position, Quaternion.identity).GetComponent<CarDriver>();
        newBus.pathFinder = pathFinder;
        newBus.GetComponent<CarDriver>().ResetPosition();
        newBus.spawner = gameObject;

        currBuses++;
    }

    void OnDrawGizmos()
    {
        if (startNode != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startNode.transform.position, checkRadius);
        }
        if (endNode != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endNode.transform.position, checkRadius);
        }
    }
}
