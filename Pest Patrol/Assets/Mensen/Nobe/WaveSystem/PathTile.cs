using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour, IPathTileInterface
{
    public bool shouldFlipWayPoints;
    public Transform[] wayPoints;
    public Transform[] flyingWaypoints;
    public Transform snapPoint;
    public int tileSize;

    public bool startingPath;

    public void Start()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            GameManager.wayPoints.Add(wayPoints[i]);
        }
        for(int i = 0;i < flyingWaypoints.Length; i++)
        {
            GameManager.flyingWaypoints.Add(flyingWaypoints[i]);
        }
    }
    public Transform GetSnapPoint()
    {
        return snapPoint;
    }
}
