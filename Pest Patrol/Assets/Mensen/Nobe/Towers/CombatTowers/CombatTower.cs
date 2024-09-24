using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (GameObject enemy in GameManager.enemies.Where(enemy => enemy != null))
        {
            distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= detectionRange)
            {
                if(enemy.GetComponent<Enemy>().type == typeToTarget || typeToTarget == EnemyType.all)
                {
                    currentDetectedEnemyGO = enemy;
                    Ticker.OnTickAction += EnemyDistanceCheck;
                    break;
                }
            }
        }
    }
    protected virtual IEnumerator Reload()
    {

        var middleMan = baseReloadSpeed * TowerManager.globalTowerReloadSpeedMultiplier;
        yield return new WaitForSeconds(middleMan);
        onReloadTime = false;
    }
    protected virtual void AimAtEnemy()
    {

    }

}
