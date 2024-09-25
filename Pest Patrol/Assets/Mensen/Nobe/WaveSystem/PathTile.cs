using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour, IPathTileInterface
{
    public bool shouldFlipWayPoints;
    public Transform[] wayPoints;
    public Transform snapPoint;
    public int tileSize;

    public bool startingPath;

    public void Start()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            GameManager.wayPoints.Add(wayPoints[i]);
        }
    }
    public Transform GetSnapPoint()
    {
        return snapPoint;
    }
}
