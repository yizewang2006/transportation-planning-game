using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class BusStationEditor : MonoBehaviour
{
    [ReadOnly] public BusSpawner currentlyEditingStation;
    [ReadOnly] public Node startingStation;
    [ReadOnly] public Node endingStation;
    [HorizontalLine]
    Camera mainCamera;
    public Transform busCircleDrawer;
    public float yOffset = 0.1f;
    public float checkRadius = 2;
    public LayerMask roadLayer;

    [HorizontalLine]
    public BusSpawner busSpawnerPrefab;
    public GameObject startCircle;
    public GameObject endCircle;
    public GameObject startingInstruction;
    public GameObject endingInstruction;
    public Transform newStationParent;

    RaycastHit hit;
    StationManager stationManager;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        stationManager = StationManager.Instance;

        startingInstruction.SetActive(false);
        endingInstruction.SetActive(false);
    }

    void Update()
    {
        BusCircleUpdate();
    }

    void BusCircleUpdate()
    {
        if (stationManager.currentMode != StationManager.Mode.BusEditor)
        {
            startingInstruction.SetActive(false);
            endingInstruction.SetActive(false);

            busCircleDrawer.gameObject.SetActive(false);
            ResetSelectionCheck();
            return;
        }
        else
        {
            startingInstruction.SetActive(startingStation == null && endingStation == null);
            endingInstruction.SetActive(startingStation != null && endingStation == null);
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, checkRadius);
            foreach (Collider collider in colliders)
            {
                Node node = collider.GetComponent<Node>();
                if (node != null && node.nodeType == Node.NodeType.BusStation)
                {
                    Vector3 hitPoint = node.transform.position + new Vector3(0f, yOffset + 0.1f, 0f);
                    busCircleDrawer.position = hitPoint;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (startingStation == null)
                        {
                            if (stationManager.busStations.Any(item => item.startNode == node)) return;
                            startingStation = node;
                            currentlyEditingStation = Instantiate(busSpawnerPrefab, node.transform.position, Quaternion.identity, newStationParent);
                            currentlyEditingStation.startNode = node;

                            stationManager.busStations.Add(currentlyEditingStation);

                            Instantiate(startCircle, hitPoint, Quaternion.identity, currentlyEditingStation.transform);
                        }
                        else if (endingStation == null && node != startingStation)
                        {
                            endingStation = node;
                            currentlyEditingStation.endNode = node;
                            Instantiate(endCircle, hitPoint, Quaternion.identity, currentlyEditingStation.transform);
                        }

                        ResetSelectionCheck();
                    }

                    break;
                }
                else
                {
                    Vector3 hitPoint = hit.point + new Vector3(0f, yOffset, 0f);
                    busCircleDrawer.position = hitPoint;
                }
            }
        }

        if (hit.transform == null) return;
        busCircleDrawer.gameObject.SetActive(((1 << hit.transform.gameObject.layer) & roadLayer) != 0);
    }

    void ResetSelectionCheck()
    {
        if (startingStation != null && endingStation != null)
        {
            startingStation = null;
            endingStation = null;
            currentlyEditingStation = null;

            startingInstruction.SetActive(false);
            endingInstruction.SetActive(false);

            stationManager.ChangeToPointerMode();

            return;
        }

        if (startingStation != null || endingStation != null)
        {
            if (stationManager.currentMode == StationManager.Mode.BusEditor) return;

            startingStation = null;
            endingStation = null;
            if (currentlyEditingStation != null)
            {
                stationManager.busStations.Remove(currentlyEditingStation);
                Destroy(currentlyEditingStation.gameObject);
                currentlyEditingStation = null;
            }

            startingInstruction.SetActive(false);
            endingInstruction.SetActive(false);
        }
    }
}
