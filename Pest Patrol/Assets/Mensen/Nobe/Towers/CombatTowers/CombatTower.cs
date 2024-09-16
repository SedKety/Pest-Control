using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CombatTower : Tower
{
    //base Stats, these will be multiplied by the towermanager class
    [Header("Attacking Tower Variables")]
    public int baseDamage;
    public float baseReloadSpeed;
    public float detectionRange;
    public GameObject currentDetectedEnemyGO;


    public Transform shootPoint;
    public GameObject projectileGO;
    public float timeToAttackEnemy;
    protected bool onReloadTime = false;

    protected virtual void EnemyDistanceCheck()
    {

    }
    protected abstract IEnumerator TryAttackEnemy();
    protected virtual void TryDetectEnemy()
    {
        if (currentDetectedEnemyGO) { return; }
        var distance = 0f;
        foreach (GameObject enemy in GameManager.instance.enemies)
        {
            if (enemy != null)
            {
                distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= detectionRange)
                {
                    currentDetectedEnemyGO = enemy;
                    Ticker.OnTickAction += EnemyDistanceCheck;
                    break;
                }
            }
        }
    }

    protected virtual void AimAtEnemy()
    {

    }

}
