using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Crossbow : CombatTower
{
    public Transform crossbowPad, crossbow;
    private Quaternion originalCrossbowPadRotation, originalCrossbowRotation;
    private bool isReturningBackToPosition;

    protected override void Start()
    {
        base.Start();
        Ticker.OnTickAction += OnTick;
        originalCrossbowPadRotation = crossbowPad.rotation;
        originalCrossbowRotation = crossbow.rotation;
        currentReloadSpeed = baseReloadSpeed;
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
            if (!isReturningBackToPosition) { ReturnToOriginalPosition(); }
        }
        else if (currentDetectedEnemyGO != null)
        {
            StartCoroutine(TryAttackEnemy());
        }
    }
    #endregion
    protected override void TryDetectEnemy()
    {
        if (currentDetectedEnemyGO || !canShoot) { return; }
        var distance = 0f;
        GameObject closestFlyingEnemy = GameManager.enemies
     .Where(enemy => enemy != null &&
     enemy.GetComponent<FlyingEnemy>() != null && Vector3.Distance(transform.position, enemy.transform.position) <= detectionRange)
     .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
     .FirstOrDefault();
        if (closestFlyingEnemy != null)
        {
            if (closestFlyingEnemy.GetComponent<Enemy>().type == typeToTarget || typeToTarget == EnemyType.all)
            {
                currentDetectedEnemyGO = closestFlyingEnemy;
                return;
            }
        }
        foreach (GameObject enemy in GameManager.enemies.Where(enemy => enemy != null))
        {
            distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= detectionRange)
            {
                if (enemy.GetComponent<Enemy>().type == typeToTarget || typeToTarget == EnemyType.all)
                {
                    currentDetectedEnemyGO = enemy;
                    Ticker.OnTickAction += EnemyDistanceCheck;
                    break;
                }
            }
        }
    }

    protected async void ReturnToOriginalPosition()
    {
        isReturningBackToPosition = true;
        while (crossbow.rotation != originalCrossbowRotation && currentDetectedEnemyGO == null)
        {
            crossbow.rotation = Quaternion.Slerp(crossbow.rotation, originalCrossbowRotation, 0.1f);
            crossbowPad.rotation = Quaternion.Slerp(crossbowPad.rotation, originalCrossbowPadRotation, 0.1f);
            await Task.Delay(20);
        }
        isReturningBackToPosition = false;
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
        if (this == null || !gameObject || !currentDetectedEnemyGO || !canShoot)
        {
            return;
        }

        var distance = Vector3.Distance(transform.position, currentDetectedEnemyGO.transform.position);

        if (distance > detectionRange)
        {
            currentDetectedEnemyGO = null;
            ReturnToOriginalPosition();
            Ticker.OnTickAction -= EnemyDistanceCheck;
        }
    }

    protected override IEnumerator TryAttackEnemy()
    {
        if (currentDetectedEnemyGO == null || onReloadTime || !canShoot)
        {
            yield return null;
            yield break;
        }
        onReloadTime = true;
        AimAtEnemy();

        audio.Play();
        Projectile arrow = Instantiate(projectileGO, shootPoint.position, shootPoint.rotation).GetComponent<Projectile>();
        arrow.enemyGO = currentDetectedEnemyGO;
        arrow.projectileDamage = currentDamage; 
        StartCoroutine(Reload());
        yield return null;
    }

    #region async bs
    void OnApplicationQuit()
    {
        #if UNITY_EDITOR
        var constructor = SynchronizationContext.Current.GetType().GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(int) }, null);
        var newContext = constructor.Invoke(new object[] { Thread.CurrentThread.ManagedThreadId });
        SynchronizationContext.SetSynchronizationContext(newContext as SynchronizationContext);
        #endif
    }
    #endregion
}
