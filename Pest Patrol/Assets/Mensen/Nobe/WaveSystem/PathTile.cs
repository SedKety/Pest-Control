using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour
{
    public bool shouldFlipWayPoints;
    public Transform[] wayPoints;
    public List<Transform> snapPoints;
    public int tileSize;
}
