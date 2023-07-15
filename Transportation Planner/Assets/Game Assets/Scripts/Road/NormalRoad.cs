using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class NormalRoad : MonoBehaviour
{
    [Header("A")]
    public Node aIn1;
    public Node aIn2;
    public Node aOut1;
    public Node aOut2;
    public Node aUTurn1;
    public Node aUTurn2;
    [HorizontalLine]
    [Header("B")]
    public Node bIn1;
    public Node bIn2;
    public Node bOut1;
    public Node bOut2;
    public Node bUTurn1;
    public Node bUTurn2;
    [HorizontalLine]
    public GameObject aText;
    public GameObject bText;
    [HorizontalLine]
    [ReadOnly] public bool hasAIntersection;
    [ReadOnly] public bool hasBIntersection;

    public enum Connection { A, B };

    NodeManager manager;

    void Start()
    {
        manager = NodeManager.Instance;

        aText.SetActive(false);
        bText.SetActive(false);
        foreach (var item in GetComponentsInChildren<MeshRenderer>()) item.enabled = false;

        hasAIntersection = manager.intersections.Any(intersection =>
        CheckConnection(intersection.wConnection, Connection.A) ||
        CheckConnection(intersection.nConnection, Connection.A) ||
        CheckConnection(intersection.sConnection, Connection.A) ||
        CheckConnection(intersection.eConnection, Connection.A));

        hasBIntersection = manager.intersections.Any(intersection =>
        CheckConnection(intersection.wConnection, Connection.B) ||
        CheckConnection(intersection.nConnection, Connection.B) ||
        CheckConnection(intersection.sConnection, Connection.B) ||
        CheckConnection(intersection.eConnection, Connection.B));

        DisableUTurn(Connection.A, hasAIntersection);
        DisableUTurn(Connection.B, hasBIntersection);

        if(!hasBIntersection) bOut1.Connect(bUTurn1);
        if(!hasAIntersection) aOut1.Connect(aUTurn1);
    }

    public void DisableUTurn(Connection connection, bool disabled)
    {
        if (connection == Connection.A)
        {
            aUTurn1.gameObject.SetActive(!disabled);
            aUTurn2.gameObject.SetActive(!disabled);
        }
        else if (connection == Connection.B)
        {
            bUTurn1.gameObject.SetActive(!disabled);
            bUTurn2.gameObject.SetActive(!disabled);
        }
    }

    bool CheckConnection(Intersection.RoadConnection roadConnection, Connection connection)
    {
        return roadConnection != null && roadConnection.road != null &&
        roadConnection.road.GetComponent<NormalRoad>() == this &&
        roadConnection.connection == connection;
    }
}
