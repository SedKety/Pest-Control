using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public void Start()
    {
        GameManager.instance.wayPoints.Add(gameObject.transform);
    }
}
