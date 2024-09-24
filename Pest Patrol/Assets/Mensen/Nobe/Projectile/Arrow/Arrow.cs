using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    public void Start()
    {
        transform.LookAt(enemyGO.transform);
    }
    protected override void Update()
    {
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime * TowerManager.globalProjectileMovementSpeedMultiplier));
        for (int i = 0; i < GameManager.enemies.Count; i++)
        {
            var distance = Vector3.Distance(transform.position, GameManager.enemies[i].transform.position);
            if (distance <= hitDistance)
            {
                var middleman = projectileDamage * TowerManager.globalTowerDamageMultiplier;
                enemyGO.GetComponent<Enemy>().OnHit((int)middleman);
                Ticker.OnTickAction -= OnTick;
                pierceCount++;
                if (pierceCount >= pierceAmount)
                {
                    Destroy(gameObject);
                }
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
