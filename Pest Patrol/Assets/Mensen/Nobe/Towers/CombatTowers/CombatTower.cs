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
    private Coroutine stunCoroutine;

    public GameObject stunParticles;
    public Transform stunParticleHolder;
    protected virtual IEnumerator StunTower(float timeInSeconds)
    {
        print(gameObject.name + " is stunned for " + timeInSeconds + " seconds");
        canShoot = false;
        currentDetectedEnemyGO = null;
       var particles = Instantiate(stunParticles, stunParticleHolder.position , Quaternion.identity); 

        yield return new WaitForSeconds(timeInSeconds);

        canShoot = true;
        print(gameObject.name + " can shoot again.");
        Destroy(particles);
    }

    public void StartStun(float timeInSeconds)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        stunCoroutine = StartCoroutine(StunTower(timeInSeconds)); 
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
            stunCoroutine = null;
        }

        Ticker.OnTickAction -= EnemyDistanceCheck;
    }
    protected virtual void EnemyDistanceCheck()
    {

    }
    protected virtual IEnumerator TryAttackEnemy()
    {
        yield return null;
    }
    protected virtual void TryDetectEnemy()
    {
        if (currentDetectedEnemyGO || !canShoot) { return; }
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
