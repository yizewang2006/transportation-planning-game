using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BusSpawnManager : MonoBehaviour
{
    public ObjectPooler busPooler;

    [HorizontalLine]
    public float busSpawnInterval;

    public static BusSpawnManager Instance;

    void Awake()
    {
        Instance = this;
    }
}
