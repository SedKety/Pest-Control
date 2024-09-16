using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Crossbow : CombatTower
{
    public Transform crossbowPad, crossbow;
    private Quaternion originalCrossbowPadRotation, originalCrossbowRotation;

    protected override void Start()
    {
        Ticker.OnTickAction += OnTick;
        print("Placed Crossbow");
        originalCrossbowPadRotation = crossbowPad.rotation;
        originalCrossbowRotation = crossbow.rotation;
    }
    #region UpdateLoops
    protected override void Update()
    {
        if (currentDetectedEnemyGO != null)
        {
            AimAtEnemy();
        }
    }
    protected override void OnTick()
    {
        TryDetectEnemy();
        if (currentDetectedEnemyGO == null)
        {
            ReturnToOriginalPosition();
        }
        else if (currentDetectedEnemyGO != null)
        {
            StartCoroutine(TryAttackEnemy());

        }
    }
    #endregion

    protected virtual void ReturnToOriginalPosition()
    {
        crossbowPad.rotation = originalCrossbowPadRotation;
        crossbow.rotation = originalCrossbowRotation;

    }

    protected override void AimAtEnemy()
    {
        if (!currentDetectedEnemyGO) { return; }
        Vector3 padTargetPos = new(currentDetectedEnemyGO.transform.position.x, crossbowPad.position.y, currentDetectedEnemyGO.transform.position.z);
        crossbowPad.LookAt(padTargetPos);

        Vector3 crosTargetPos = currentDetectedEnemyGO.transform.position;
        crossbow.LookAt(crosTargetPos);
    }

    protected override void EnemyDistanceCheck()
    {
        if (!currentDetectedEnemyGO) { Ticker.OnTickAction -= EnemyDistanceCheck; return; }
        var distance = 0f;
        distance = Vector3.Distance(transform.position, currentDetectedEnemyGO.transform.position);
        if (distance > detectionRange)
        {
            currentDetectedEnemyGO = null;
            ReturnToOriginalPosition();
            Ticker.OnTickAction -= EnemyDistanceCheck;
        }
    }
    protected override IEnumerator TryAttackEnemy()
    {
        if (currentDetectedEnemyGO == null || onReloadTime)
        {
            yield return null;
            yield break;  
        }
        onReloadTime = true;

        Instantiate(projectileGO, shootPoint.position, shootPoint.rotation);

        if (currentDetectedEnemyGO != null)
        {
            StartCoroutine(DamageEnemy(currentDetectedEnemyGO.GetComponent<Enemy>()));
        }

        var middleMan = baseReloadSpeed * TowerManager.globalTowerReloadSpeedMultiplier;
        yield return new WaitForSeconds(middleMan);
        onReloadTime = false;

        if (currentDetectedEnemyGO != null)
        {
            StartCoroutine(TryAttackEnemy());
        }

        yield return null;
    }
    protected IEnumerator DamageEnemy(Enemy enemy)
    {
        if(enemy == null) {  yield return null; }   
        yield return new WaitForSeconds(timeToAttackEnemy);
        if (enemy == null) { yield return null; }
        var middleman = baseDamage * TowerManager.globalTowerDamageMultiplier;
        enemy.OnHit((int)middleman);
        yield return null;
    }
}
