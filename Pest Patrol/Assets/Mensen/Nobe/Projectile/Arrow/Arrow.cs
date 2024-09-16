using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    protected override void Update()
    {
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime * TowerManager.globalProjectileMovementSpeedMultiplier));
    }
    protected override void OnTick()
    {
        currentTickCount++; 
        if (currentTickCount >= tickLifeTime)
        {
            Ticker.OnTickAction -= OnTick;
            Destroy(gameObject);
        }
    }
}
