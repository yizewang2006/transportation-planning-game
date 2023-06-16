using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BusRoute : MonoBehaviour
{
    public List<Node> busStations = new List<Node>();
    public List<PathFinder> pathFinders = new List<PathFinder>();
    public List<Node> bestPathAll = new List<Node>();
    public float lineWidth;

    [Space(20)]
    public float iconYOffset = 2;
    public float circleIconYOffset = 0.01f;
    public Transform disconnectedIcon;
    public Transform stationIcon;
    public Transform circleIcon;
    public Transform busStationHolder;
    public Material lineMaterial;

    GameObject newRoute;
    void Start()
    {
        newRoute = new GameObject("Bus Station " + (busStationHolder.childCount + 1));
        newRoute.transform.SetParent(busStationHolder);

        LineRenderer newLine = newRoute.AddComponent<LineRenderer>();
        newLine.alignment = LineAlignment.TransformZ;
        newLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        newLine.receiveShadows = false;
        newLine.transform.rotation = Quaternion.Euler(90,0,0);
        newLine.startWidth = lineWidth;
        newLine.endWidth = lineWidth;
        newLine.material = lineMaterial;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) UpdateBusStation();
        if(pathFinders.Count > 0)
        {
            bestPathAll = pathFinders.SelectMany(pf => pf.bestPath).ToList();
            newRoute.GetComponent<LineRenderer>().positionCount = bestPathAll.Count;
            newRoute.GetComponent<LineRenderer>().SetPositions(bestPathAll.Select(node => node.transform.position + (Vector3.up * 0.2f)).ToArray());

        }
    }

    public void UpdateBusStation()
    {
        DestroyAllRoutes(busStationHolder);
        // If no connection on one end
        if(busStations.Count == 1)
        {
            SpawnIcon(false, busStations[0].transform);
            Instantiate(circleIcon, busStations[0].transform.position + (Vector3.up * circleIconYOffset), Quaternion.Euler(90, 0, 0))
            .SetParent(newRoute.transform);
        }
        // If no connection in both ends 
        else if(busStations.Count > 1 && busStations[0] != busStations[^1])
        {
            for (int i = 0; i < busStations.Count; i++)
            {
                if(i == 0 || i == busStations.Count - 1)
                {
                    SpawnIcon(false, busStations[i].transform);
                }
            }

            for (int i = 0; i < busStations.Count; i++)
            {
                if(i != busStations.Count - 1)
                {
                    PathFinder newPath = newRoute.AddComponent<PathFinder>();
                    PathFinder.AddAllNodes(newPath);
                    newPath.start = busStations[i];
                    newPath.end = busStations[i+1];
                    
                    pathFinders.Add(newPath);
                }
                Instantiate(circleIcon, busStations[i].transform.position + (Vector3.up * circleIconYOffset), Quaternion.Euler(90, 0, 0))
                .SetParent(newRoute.transform);
            }
        } 
        // Connection in every station
        else 
        {
            for (int i = 0; i < busStations.Count; i++)
            {
                SpawnIcon(true, busStations[i].transform);
            }
        }
    }

    void SpawnIcon(bool connected, Transform station)
    {
        Instantiate(connected? stationIcon : disconnectedIcon, station.transform.position + (Vector3.up * iconYOffset), Quaternion.identity).SetParent(newRoute.transform);
    }

    void DestroyAllRoutes(Transform parent)
    {
        if(parent.childCount == 0) return;
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }
}
