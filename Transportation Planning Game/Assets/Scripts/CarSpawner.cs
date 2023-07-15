using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public PathFinder carPath;
    public PathFinder busPath;
    public int currentPeople;
    public int maxPeople;
    public int peoplePerCar;
    public float spawnIntervalCar;

    int peoplePerBus;
    int maxBusPerRoad;
    float spawnIntervalBus;
    
    public List<GameObject> carPrefab = new List<GameObject>();
    public List<GameObject> busPrefab = new List<GameObject>();

    bool canSpawn;
    BusRouteManager busRouteManager;

    void Start()
    {
        canSpawn = true;
        busRouteManager = BusRouteManager.Instance;
        peoplePerBus = busRouteManager.peoplePerBus;
        maxBusPerRoad = busRouteManager.maxBusPerRoad;
        spawnIntervalBus = busRouteManager.busSpawnInterval;
        busPrefab =  new List<GameObject>(busRouteManager.busPrefab);

        currentPeople = maxPeople;
        InvokeRepeating("SpawnCarInterval", 0, spawnIntervalCar);
        InvokeRepeating("SpawnBusInterval", 0, spawnIntervalBus);
        InvokeRepeating("CheckIfReset", 0, 0.1f);
    }

    public void SpawnCarInterval() {
        if(!canSpawn) return;
        //do we have enough people for a car?
        if (currentPeople >= peoplePerCar) {
            SpawnCar(carPath);
            currentPeople -= peoplePerCar;
        } else 
        {
            SpawnCar(carPath);
            currentPeople = 0;
        }
    }

    public void SpawnBusInterval() {
        if(busPath == null) return;
        //do we have enough people for a bus?
        if (currentPeople >= peoplePerBus) {
            canSpawn = false;
            SpawnCar(busPath);
            currentPeople -= peoplePerBus;
        } else 
        {
            canSpawn = true;
        }
    }

    public void CheckIfReset() {
        //reset currentPeople if it reaches 0.
        //though this means a road spawns infinite vehicles, maintaining a currentPeople count is neccessary to determine
        //if a bus needs to be spawned or if a car needs to be spawned.
        if (currentPeople <= 0) {
            currentPeople = maxPeople;
        }
    }

    void SpawnCar(PathFinder p)
    {
        GameObject prefab = p == carPath? carPrefab[Random.Range(0, carPrefab.Count)] : busPrefab[Random.Range(0, busPrefab.Count)];
        CarDriver car = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<CarDriver>();
        car.pathNodes.Add(p);
        car.ResetPosition();
    }
}
