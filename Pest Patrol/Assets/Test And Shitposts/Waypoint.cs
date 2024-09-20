using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public void Start()
    {
        GameManager.wayPoints.Add(gameObject.transform);
    }
}
