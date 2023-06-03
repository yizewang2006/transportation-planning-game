using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public List<Intersection> connectedNodes = new List<Intersection>();
    public bool searched;
}
