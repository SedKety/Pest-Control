using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankEnemy : Enemy
{
    public int shield;
    public ProjectileType typeToHitShield;
    public override void OnHit(int damage, ProjectileType projectileType)
    {
        if (shield > 0)
        {
            if (projectileType != typeToHitShield)
            {
                return;
            }
            if (damage >= shield)
            {
                int leftOverDamage = damage - shield;
                shield = 0;
                OnShieldBreak();
                health -= leftOverDamage;
            }
            else
            {
                shield -= damage;
            }
        }
        else
        {
            health -= damage;
        }
        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }

        print(shield + " " + health);
    }




    protected abstract void OnShieldBreak();
}
