using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
public class RoadPathIndicator : MonoBehaviour
{
    PathFinder pathFinder;
    [ReadOnly][SerializeField] bool selected;
    [HorizontalLine]
    public LineRenderer line;
    public float lineWidth;
    public Material[] materials;

    bool createLine;

    StationManager stationManager;

    void Start()
    {
        stationManager = StationManager.Instance;
        pathFinder = GetComponent<PathFinder>();
    }

    void Update()
    {
        if (pathFinder.IsPathCalculated() && !createLine)
        {
            createLine = true;
            line.positionCount = pathFinder.bestPath.Count;
            line.SetPositions(pathFinder.bestPath.Select(node => node.transform.position + (Vector3.up * 0.2f)).ToArray());
            line.transform.rotation = Quaternion.Euler(90, 0, 0);
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.material = materials[Random.Range(0, materials.Length)];
        }

        if(GetComponent<CarSpawner>() != null)
            selected = stationManager.currentCarSelected == this.GetComponent<CarSpawner>() || stationManager.showCarPathToggle.isOn;
        else if(GetComponent<BusSpawner>() != null)
            selected = stationManager.currentBusSelected == this.GetComponent<BusSpawner>() || stationManager.showBusPathToggle.isOn;
        else
            selected = false;
        line.enabled = selected;
    }

}
