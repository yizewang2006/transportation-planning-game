using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BusRouteManager : MonoBehaviour
{
    public BusRoute currentEditingRoute;
    public Node currentSelectedNode;
    public float mouseRayCheck;

    [Space(20)]
    public float detectTerminalRange;
    public int maxBusPerRoad = 2;
    public int peoplePerBus;
    public float busSpawnInterval;
    public List<GameObject> busPrefab = new List<GameObject>();

    [Space(20)]
    public TMP_Text drawingText;
    public GameObject circleSelection;
    public float lineWidth;
    public float iconYOffset = 2;
    public float circleIconYOffset = 0.01f;
    public Transform stationIcon;
    public Transform circleIcon;
    public Transform busStationHolder;
    public Material lineMaterial;
    public Material startStationMat;
    public Material endStationMat;

    Dictionary<Node, GameObject> circleIcons = new Dictionary<Node, GameObject>();

    RaycastHit hit;
    public static BusRouteManager Instance;

    bool drawing;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        drawingText.text = drawing ? "Done" : "Draw Bus Route";
        circleSelection.SetActive(drawing);

        if (drawing)
        {
            BusPathCreator();
            UpdateCircleSelection();
        }
    }

    void BusPathCreator()
    {
        if (Input.GetMouseButtonDown(0))
    {
        if (currentEditingRoute == null)
        {
            currentEditingRoute = CreateNewBusRoute();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, mouseRayCheck);
            float closestDistance = float.MaxValue;
            Node closestNode = null;

            foreach (Collider collider in colliders)
            {
                Node node = collider.GetComponent<Node>();

                if (node != null && node.nodeType == Node.NodeType.Bus)
                {
                    float distance = Vector3.Distance(hit.point, collider.ClosestPoint(hit.point));

                    if (distance < closestDistance)
                    {
                        closestNode = node;
                        closestDistance = distance;
                    }
                }
            }

            if (closestNode != null)
            {
                if (currentEditingRoute.busStations.Count == 2)
                {
                    Node firstNode = currentEditingRoute.busStations[0];
                    currentEditingRoute.busStations.RemoveAt(0);
                    DeleteCircleIcon(firstNode); // Delete circleIcon of the replaced node
                }

                if (currentEditingRoute.busStations.Contains(closestNode))
                {
                    currentEditingRoute.busStations.Remove(closestNode);
                    DeleteCircleIcon(closestNode); // Delete circleIcon if the node is removed
                }
                else
                {
                    currentEditingRoute.busStations.Add(closestNode);
                    SpawnCircleIcon(closestNode); // Spawn circleIcon if the node is added
                }
            }
        }
    }
    }

    void UpdateCircleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            circleSelection.transform.position = hit.point + (Vector3.up * circleIconYOffset);
        }
    }


    public void StartDrawing()
    {
        if (drawing)
        {
            if (currentEditingRoute != null)
            {
                if(currentEditingRoute.busStations.Count < 2) return;
                CreateBusLines(currentEditingRoute);

                currentEditingRoute = null;
            }
        }
        else
        {
            currentEditingRoute = CreateNewBusRoute();
        }

        drawing = !drawing;
    }

    BusRoute CreateNewBusRoute()
    {
        return busStationHolder.AddComponent<BusRoute>();
    }

    public void CreateBusLines(BusRoute busRoute)
    {
        busRoute.CreateBusLine();
    }



    void SpawnCircleIcon(Node node)
    {
        if (!circleIcons.ContainsKey(node))
        {
            GameObject circleIconObj = Instantiate(circleIcon.gameObject, node.transform.position + (Vector3.up * circleIconYOffset), Quaternion.Euler(90, 0, 0));
            circleIconObj.transform.SetParent(currentEditingRoute.newRoute.transform);
            circleIcons[node] = circleIconObj;
        }
    }

    void DeleteCircleIcon(Node node)
    {
        if (circleIcons.ContainsKey(node))
        {
            Destroy(circleIcons[node]);
            circleIcons.Remove(node);
        }
    }

}
