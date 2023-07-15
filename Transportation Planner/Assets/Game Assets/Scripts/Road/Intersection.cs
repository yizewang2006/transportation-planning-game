using NaughtyAttributes;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public RoadConnection wConnection;
    public RoadConnection eConnection;
    public RoadConnection nConnection;
    public RoadConnection sConnection;
    [HorizontalLine]
    public Node nwCorner;
    public Node neCorner;
    public Node seCorner;
    public Node swCorner;
    public Node center;
    [HorizontalLine]
    public GameObject[] textIndicator;

    void Start()
    {
        Init();
        SetupConnections();
    }

    void Init()
    {
        foreach (var item in textIndicator) item.SetActive(false);

        wConnection.SetEnds();
        eConnection.SetEnds();
        nConnection.SetEnds();
        sConnection.SetEnds();
    }

    void SetupConnections()
    {
        ConnectIntersection(wConnection, nConnection, nwCorner);
        ConnectIntersection(wConnection, sConnection, swCorner);
        ConnectIntersection(wConnection, eConnection, null);

        ConnectIntersection(nConnection, eConnection, neCorner);
        ConnectIntersection(nConnection, wConnection, nwCorner);
        ConnectIntersection(nConnection, sConnection, null);

        ConnectIntersection(eConnection, sConnection, seCorner);
        ConnectIntersection(eConnection, nConnection, neCorner);
        ConnectIntersection(eConnection, wConnection, null);

        ConnectIntersection(sConnection, wConnection, swCorner);
        ConnectIntersection(sConnection, eConnection, seCorner);
        ConnectIntersection(sConnection, nConnection, null);
    }

    void ConnectIntersection(RoadConnection connection1, RoadConnection connection2, Node cornerNode)
    {
        bool isConnection2Left = (connection1 == wConnection && connection2 == sConnection) ||
                                (connection1 == sConnection && connection2 == eConnection) ||
                                (connection1 == eConnection && connection2 == nConnection) ||
                                (connection1 == nConnection && connection2 == wConnection);

        if (connection1.road != null && connection2.road != null)
        {
            if (cornerNode != null)
            {
                if (isConnection2Left)
                {
                    connection1.endOut1.Connect(cornerNode);
                    cornerNode.Connect(connection2.endIn1);

                    connection2.endOut2.Connect(center);
                    center.Connect(connection1.endIn2);
                    center.Connect(connection1.endIn1);
                }
                else
                {
                    connection2.endOut1.Connect(cornerNode);
                    cornerNode.Connect(connection1.endIn1);

                    connection1.endOut2.Connect(center);
                    center.Connect(connection2.endIn2);
                    center.Connect(connection2.endIn1);
                }
            }
            else
            {
                connection1.endOut2.Connect(connection2.endIn2);
                connection2.endOut2.Connect(connection1.endIn2);
                connection1.endOut1.Connect(connection2.endIn1);
                connection2.endOut1.Connect(connection1.endIn1);
            }
        }
    }


    [System.Serializable]
    public class RoadConnection
    {
        public NormalRoad road;
        public NormalRoad.Connection connection;

        [AllowNesting][ReadOnly] public Node endIn1;
        [AllowNesting][ReadOnly] public Node endIn2;
        [AllowNesting][ReadOnly] public Node endOut1;
        [AllowNesting][ReadOnly] public Node endOut2;

        public void SetEnds()
        {
            if (road == null) return;
            if (connection == NormalRoad.Connection.A)
            {
                endIn1 = road.aIn1;
                endIn2 = road.aIn2;

                endOut1 = road.aOut1;
                endOut2 = road.aOut2;
            }
            else
            {
                endIn1 = road.bIn1;
                endIn2 = road.bIn2;

                endOut1 = road.bOut1;
                endOut2 = road.bOut2;
            }
        }
    }
}
