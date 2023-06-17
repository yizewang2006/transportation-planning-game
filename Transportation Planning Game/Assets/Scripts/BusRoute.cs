using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BusRoute : MonoBehaviour
{
    public List<Node> busStations = new List<Node>();

    float lineWidth;
    float iconYOffset = 2;
    float circleIconYOffset = 0.01f;
    Transform stationIcon;
    Transform circleIcon;
    Transform busStationHolder;
    Material lineMaterial;

    public GameObject newRoute = null;
    BusRouteManager busRouteManager;
    PathFinder newPath;
    void Start()
    {
        busRouteManager = BusRouteManager.Instance;
        busStationHolder = busRouteManager.busStationHolder;

        newRoute = new GameObject("Bus Station " + (busStationHolder.childCount + 1));
        newRoute.transform.SetParent(busStationHolder);

        SetupLine();
    }

    void SetupLine()
    {
        lineWidth = busRouteManager.lineWidth;
        iconYOffset = busRouteManager.iconYOffset;
        circleIconYOffset = busRouteManager.circleIconYOffset;
        stationIcon = busRouteManager.stationIcon;
        circleIcon = busRouteManager.circleIcon;
        lineMaterial = busRouteManager.lineMaterial;

        LineRenderer newLine = newRoute.AddComponent<LineRenderer>();
        newLine.alignment = LineAlignment.TransformZ;
        newLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        newLine.receiveShadows = false;
        newLine.transform.rotation = Quaternion.Euler(90, 0, 0);
        newLine.startWidth = lineWidth;
        newLine.endWidth = lineWidth;
        newLine.material = lineMaterial;
        newLine.numCornerVertices = 5;
    }

    void Update()
    {
        busStations.RemoveAll(node => node == null);

        if(newRoute.transform.childCount > 0)
        {
            newRoute.transform.GetChild(0).GetComponent<MeshRenderer>().material = busRouteManager.startStationMat;
            
            if(newRoute.transform.childCount > 1)
                newRoute.transform.GetChild(1).GetComponent<MeshRenderer>().material = busRouteManager.endStationMat;
        }

        if (newPath != null)
        {
            
            newRoute.GetComponent<LineRenderer>().positionCount = newPath.bestPath.Count;
            newRoute.GetComponent<LineRenderer>().SetPositions(newPath.bestPath.Select(node => node.transform.position + (Vector3.up * 0.2f)).ToArray());
        }
    }

    public void CreateBusLine()
    {
        newPath = newRoute.AddComponent<PathFinder>();
        PathFinder.AddAllNodes(newPath);
        newPath.start = busStations[0];
        newPath.end = busStations[1];

        SpawnIcon(newPath.start.transform);
    }

    void SpawnIcon(Transform station)
    {
        Instantiate(stationIcon, station.transform.position + (Vector3.up * iconYOffset), Quaternion.identity).SetParent(busStationHolder.transform);
    }
}
