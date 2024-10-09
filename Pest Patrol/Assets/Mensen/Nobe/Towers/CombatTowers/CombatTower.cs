using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CombatTower : Tower
{
    [Header("Attacking Tower Variables")]
    public int baseDamage;
    private float baseDamageMultiplier;

    public float baseReloadSpeed;
    private float baseReloadSpeedMultiplier;
    public float detectionRange;
    public GameObject currentDetectedEnemyGO;


    public Transform shootPoint;
    public GameObject projectileGO;
    public float timeToAttackEnemy;
    protected bool onReloadTime = false;
    private Coroutine stunCoroutine;

    public GameObject stunParticles;


    public int maxAllowedDamageIncrease;
    public int currentDamageincreasePurchased;

    public int maxAllowedReloadSpeed;
    public int currentReloadSpeedincreasePurchased;

    public void IncreaseDamage(float damageMultiplier)
    {
        baseDamageMultiplier += damageMultiplier;
        baseDamage = (int)(baseDamage * baseDamageMultiplier);
        currentDamageincreasePurchased++;
    }

    public void IncreaseReloadSpeed(float reloadSpeedMultiplier)
    {
        baseReloadSpeedMultiplier -= reloadSpeedMultiplier;
        baseReloadSpeed = (int)(baseReloadSpeed * baseReloadSpeedMultiplier);
        currentReloadSpeedincreasePurchased++;
    }
    protected virtual IEnumerator StunTower(float timeInSeconds)
    {
        print(gameObject.name + " is stunned for " + timeInSeconds + " seconds");
        canShoot = false;
        currentDetectedEnemyGO = null;
        stunParticles.SetActive(true);

        yield return new WaitForSeconds(timeInSeconds);

        canShoot = true;
        print(gameObject.name + " can shoot again.");
        stunParticles.SetActive(false);
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
