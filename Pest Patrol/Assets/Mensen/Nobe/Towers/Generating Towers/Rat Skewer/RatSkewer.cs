using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSkewer : GeneratingTower
{
    public GameObject ratGO;
    public Vector3 ratRot;
    protected override void OnTick()
    {
        
    }

    protected override void Update()
    {
        ratGO.transform.Rotate(ratRot * Time.deltaTime);
    }
}
