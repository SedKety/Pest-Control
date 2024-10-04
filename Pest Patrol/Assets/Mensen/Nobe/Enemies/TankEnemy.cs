using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankEnemy : Enemy
{
    public int shield;

    public override void OnHit(int damage)
    {
        // If damage is greater than or equal to the shield
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
