using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    public Material normalMat;
    public Material busStationMat;
    public Material carStationMat;
    public Material endStationMat;

    [HorizontalLine]
    public List<Node> nodes;
    public List<Node> carStationNodes;
    public List<Node> endStationNodes;
    public List<Intersection> intersections;

    [HorizontalLine]
    public Toggle showCarToggle;
    public Toggle showBusToggle;

    [HorizontalLine]
    [SerializeField] bool showNodes;
    [SerializeField] bool showCarNodes;
    [SerializeField] bool showBusNodes; 

    public static NodeManager Instance;

    void Awake()
    {
        Instance = this;
        intersections = FindObjectsOfType<Intersection>().ToList();
        nodes = FindObjectsOfType<Node>().ToList();
        
        foreach (var item in nodes)
        {
            if(item.nodeType == Node.NodeType.Normal) item.GetComponent<MeshRenderer>().sharedMaterial = normalMat;
            else if(item.nodeType == Node.NodeType.CarStation) item.GetComponent<MeshRenderer>().sharedMaterial = carStationMat;
            else if(item.nodeType == Node.NodeType.BusStation) item.GetComponent<MeshRenderer>().sharedMaterial = busStationMat;
            else if(item.nodeType == Node.NodeType.EndStation) item.GetComponent<MeshRenderer>().sharedMaterial = endStationMat;
        }

        carStationNodes = nodes.Where(n => n.nodeType == Node.NodeType.CarStation).ToList();
        endStationNodes = nodes.Where(n => n.nodeType == Node.NodeType.EndStation).ToList();
    }

    void Update()
    {
        foreach (var item in nodes)
        {
            if(item.nodeType == Node.NodeType.Normal) item.GetComponent<MeshRenderer>().enabled = showNodes;
            if(item.nodeType == Node.NodeType.CarStation ||
            item.nodeType == Node.NodeType.EndStation) item.GetComponent<MeshRenderer>().enabled = showCarNodes;
            if(item.nodeType == Node.NodeType.BusStation) item.GetComponent<MeshRenderer>().enabled = showBusNodes;
        }

        showCarNodes = showCarToggle.isOn;
        showBusNodes = showBusToggle.isOn;
    }

    public static void AddAllNodes(NodeManager nodeManager)
    {
        nodeManager.nodes.Clear();

        Node[] nodes = FindObjectsOfType<Node>();

        foreach (Node node in nodes)
        {
            if (!nodeManager.nodes.Contains(node))
            {
                nodeManager.nodes.Add(node);
            }
        }

    }

    public int CarStationCount() => nodes.Count(n => n.nodeType == Node.NodeType.CarStation);
}
