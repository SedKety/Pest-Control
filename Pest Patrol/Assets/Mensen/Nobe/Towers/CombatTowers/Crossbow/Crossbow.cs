using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : Tower
{
    public Transform crossbowPad, crossbow;
    private Quaternion originalCrossbowPadRotation, originalCrossbowRotation;

    protected override void Start()
    {
        Ticker.OnTickAction += OnTick;
        print("Placed Crossbow");
        originalCrossbowPadRotation = crossbowPad.rotation;
        originalCrossbowRotation = crossbow.rotation;
    }
    #region UpdateLoops
    protected override void Update()
    {
        if (currentDetectedEnemy != null)
        {
            AimAtEnemy();
        }
    }
    protected override void OnTick()
    {
        TryDetectEnemy();
        if (currentDetectedEnemy == null)
        {
            ReturnToOriginalPosition();
        }
    }
    #endregion

    protected virtual void ReturnToOriginalPosition()
    {
        crossbowPad.rotation = originalCrossbowPadRotation;
        crossbow.rotation = originalCrossbowRotation;

    }
    protected override void AimAtEnemy()
    {
        if (!currentDetectedEnemy) { return; }
        Vector3 padTargetPos = new(currentDetectedEnemy.transform.position.x, crossbowPad.position.y, currentDetectedEnemy.transform.position.z);
        crossbowPad.LookAt(padTargetPos);

        Vector3 crosTargetPos = currentDetectedEnemy.transform.position;
        crossbow.LookAt(crosTargetPos);
    }
    protected override void EnemyDistanceCheck()
    {
        if (!currentDetectedEnemy) { Ticker.OnTickAction -= EnemyDistanceCheck; return; }
        print("ngh ~harder daddy~");
        var distance = 0f;
        distance = Vector3.Distance(transform.position, currentDetectedEnemy.transform.position);
        if (distance > detectionRange)
        {
            currentDetectedEnemy = null;
            ReturnToOriginalPosition();
            Ticker.OnTickAction -= EnemyDistanceCheck;
        }


    }
}
