using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject[] prefabs;
    public int initialPoolSize = 5;
    public int poolIncrement = 3;

    [HorizontalLine]
    [ReadOnly][SerializeField] List<GameObject> pool;
    [ReadOnly][SerializeField] List<GameObject> allObjects;
    [ReadOnly]int currentIndex = 0;

    void Start()
    {
        pool = new List<GameObject>();

        PopulatePool(initialPoolSize);
    }
    GameObject InstantiateObj()
    {
        GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        obj.SetActive(false);
        return obj;
    }

    void PopulatePool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newObj = InstantiateObj();
            pool.Add(newObj);
            allObjects.Add(newObj);
        }
    }

    public GameObject GrabFromPool(Vector3 position, Quaternion rotation)
    {
        // Check if the pool is empty
        if (pool.Count == 0)
            PopulatePool(poolIncrement);

        GameObject obj = GrabFromBottom();

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        obj.SetActive(true);

        return obj;
    }

    public void InsertToPool(GameObject obj)
    {
        obj.SetActive(false);
        InsertAtTop(obj);
    }

    void InsertAtTop(GameObject gameObject)
    {
        // Insert the gameObject at the beginning of the list
        pool.Insert(0, gameObject);
    }

    GameObject GrabFromBottom()
    {
        // Check if the list is empty
        if (pool.Count == 0)
        {
            return null;
        }

        // Get the gameObject from the bottom of the list
        GameObject gameObject = pool[^1];

        // Remove the gameObject from the list
        pool.RemoveAt(pool.Count - 1);

        return gameObject;
    }
}
