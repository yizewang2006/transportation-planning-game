using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    ObjectPooler carPool;
    CarSpawnManager spawnManager;
    public Node startNode;
    public Node endNode;
    [HorizontalLine]
    [ReadOnly] public int currPeople;
    [ReadOnly] public int maxPeople;
    public Vector2 maxPeopleRange;
    public int peoplePerCar;
    [ReadOnly] public int currCars;
    [ReadOnly] public int maxCars;
    public Vector2 maxCarsRange;
    
    void Start()
    {
        spawnManager = CarSpawnManager.Instance;
        carPool = spawnManager.carPooler;

        maxCars = Random.Range((int)maxCarsRange.x, (int)maxCarsRange.y + 1);
        maxPeople = Random.Range((int)maxPeopleRange.x, (int)maxPeopleRange.y + 1);
        currPeople = maxPeople;

        StartCoroutine(PopulatePeople());
        InvokeRepeating("SpawnCar", 0, spawnManager.carSpawnInterval);
    }

    void Update()
    {
        if(currCars <= 0) currCars = 0;
        if(currPeople <= 0) currPeople = 0;
        if(currPeople >= maxPeople) currPeople = maxPeople;
        if(currCars >= maxCars) currCars = maxCars;
    }

    IEnumerator PopulatePeople()
    {
        currPeople += Random.Range((int)spawnManager.peopleIncreaseCount.x, (int)spawnManager.peopleIncreaseCount.y);
        float randInterval = Random.Range(spawnManager.peopleIncreaseInterval.x, spawnManager.peopleIncreaseInterval.y);
        yield return new WaitForSeconds(randInterval);
        StartCoroutine(PopulatePeople());
    }

    void SpawnCar()
    {
        if(currCars >= maxCars) return;
        if(currPeople <= 0) return;

        if(currPeople >= peoplePerCar) currPeople -= peoplePerCar;
        else currPeople -= currPeople;
        PathFinder pathFinder = GetComponent<PathFinder>();
        CarDriver newCar = carPool.GrabFromPool(startNode.transform.position, Quaternion.identity).GetComponent<CarDriver>();
        newCar.pathFinder = pathFinder;
        pathFinder.start = startNode;
        pathFinder.end = endNode;
        newCar.GetComponent<CarDriver>().ResetPosition();
        newCar.spawner = gameObject;

        currCars++;
    }
}