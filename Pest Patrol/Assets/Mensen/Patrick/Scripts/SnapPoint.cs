using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour, IPathTileInterface
{
    public Transform snapPoint;

    public Transform GetSnapPoint()
    {
        return snapPoint;
    }
}
