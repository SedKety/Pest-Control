using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankEnemy : Enemy
{
    public int shield;
    public ProjectileType typeToHitShield;
    public override void OnHit(int damage, ProjectileType projectileType)
    {
        if (projectileType == ProjectileType.sharp && shield <= 0) { return; } 
        if (damage >= shield)
        {
            int leftOverDamage = damage - shield;
            shield = 0;
            OnShieldBreak(); 
            health -= leftOverDamage; 

            if (health <= 0)
            {
                isDead = true;
                Destroy(gameObject);
            }
        }
        else
        {
            shield -= damage;
        }

        print(shield + " " + health);
    }


    protected abstract void OnShieldBreak();
}
