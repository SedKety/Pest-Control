using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Arrow
{
    public GameObject explosion;
    protected override void Update()
    {
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime * TowerManager.globalProjectileMovementSpeedMultiplier));
        for (int i = 0; i < GameManager.enemies.Count; i++)
        {
            if (hitEnemy.Contains(GameManager.enemies[i])) {  return; }

            var distance = Vector3.Distance(transform.position, GameManager.enemies[i].transform.position);
            if (distance <= hitDistance)
            {
                GameManager.enemies[i].GetComponent<Enemy>().OnHit((int)(projectileDamage * TowerManager.globalTowerDamageMultiplier), projectileType);
                hitEnemy.Add(GameManager.enemies[i]);

                Instantiate(explosion, GameManager.enemies[i].transform.position, new());

                Ticker.OnTickAction -= OnTick;

                pierceCount++;
                if (pierceCount > pierceAmount)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
