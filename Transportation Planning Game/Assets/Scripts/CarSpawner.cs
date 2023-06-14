using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public PathFinder path;
    public int currentPeople;
    public int maxPeople;
    public int peoplePerCar;
    public float spawnInterval;
    public List<GameObject> carPrefab = new List<GameObject>();

    void Start()
    {
        currentPeople = maxPeople;
        InvokeRepeating("SpawnCarInterval", 0, spawnInterval);
        InvokeRepeating("CheckIfReset", 0, 0.1f);
    }

    public void SpawnCarInterval() {
        //do we have enough people for a car?
        if (currentPeople >= peoplePerCar) {
            SpawnCar();
            currentPeople -= peoplePerCar;
        } else 
        {
            SpawnCar();
            currentPeople = 0;
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

    void SpawnCar()
    {
        CarDriver car = Instantiate(carPrefab[Random.Range(0, carPrefab.Count)], Vector3.zero, Quaternion.identity).GetComponent<CarDriver>();
        car.pathNodes.Add(path);
        car.ResetPosition();
    }
}
