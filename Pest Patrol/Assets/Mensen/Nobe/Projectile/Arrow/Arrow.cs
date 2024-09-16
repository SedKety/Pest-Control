using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    protected override void Update()
    {
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime * TowerManager.globalProjectileMovementSpeedMultiplier));
        if(enemyGO != null)
        {
            var distance = Vector3.Distance(transform.position, enemyGO.transform.position);
            if (distance <= hitDistance)
            {
                var middleman = projectileDamage * TowerManager.globalTowerDamageMultiplier;
                enemyGO.GetComponent<Enemy>().OnHit((int)middleman);
                Ticker.OnTickAction -= OnTick;
                Destroy(gameObject);
            }
        }
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
