using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class CarDriver : MonoBehaviour
{
    [ReadOnly] public GameObject spawner;
    public UnityEvent OnReachEnd;
    [HorizontalLine]
    [ReadOnly] public Node[] bestPath;
    public float turnSmoothness = 3f;
    public Vector2 speedRange = new(2, 5);
    [ReadOnly] public int currIndex = 0;

    [HorizontalLine]

    public Color[] randomColors;

    [HideInInspector] public PathFinder pathFinder;
    float speed;

    void Start()
    {
        speed = Random.Range(speedRange.x, speedRange.y);

        if (randomColors.Length == 0) return;
        Material newMaterial = new Material(GetComponentInChildren<MeshRenderer>().sharedMaterial);
        newMaterial.color = randomColors[Random.Range(0, randomColors.Length)];
        GetComponentInChildren<MeshRenderer>().material = newMaterial;
    }

    void Update()
    {
        if(pathFinder == null)
        {
            DestroyCar();
        }
        CarMovement();
    }

    void CarMovement()
    {
        if(pathFinder == null) return;
        if (bestPath.Length < 2 || bestPath.Length != pathFinder.bestPath.Count)
        {
            bestPath = pathFinder.bestPath.ToArray();
            ResetPosition();
        }
        if (!pathFinder.IsPathCalculated()) return;

        Vector3 targetPosition = new Vector3(bestPath[currIndex].transform.position.x, transform.position.y, bestPath[currIndex].transform.position.z);
        Quaternion targetRotation = transform.rotation;

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                            new Vector2(targetPosition.x, targetPosition.z)) > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
            targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSmoothness * Time.deltaTime);
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = targetRotation;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        }

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                            new Vector2(targetPosition.x, targetPosition.z)) < 0.1f)
        {
            if (currIndex != bestPath.Length - 1)
                currIndex++;
            else
            {
                OnReachEnd?.Invoke();
                currIndex = bestPath.Length - 1;

                DestroyCar();
            }
        }

    }

    public void ResetPosition()
    {
        currIndex = 0;

        if (bestPath.Length > 0)
            transform.position = bestPath[0].transform.position + Vector3.up;
    }

    void DestroyCar()
    {
        if(spawner != null)
        {
            if(spawner.GetComponent<CarSpawner>() != null)
                spawner.GetComponent<CarSpawner>().currCars--;
            else if(spawner.GetComponent<BusSpawner>() != null)
                spawner.GetComponent<BusSpawner>().currBuses--;
            spawner = null;
        }

        ResetPosition();
        bestPath.ToList().Clear();
        CarSpawnManager.Instance.carPooler.InsertToPool(gameObject);
    }
}
