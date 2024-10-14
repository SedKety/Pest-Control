using System.Collections;
using UnityEngine;
public class Mortar : CombatTower
{
    public GameObject explosionGO;
    public float timeTillNextExplosion;
    protected override void Start()
    {
        Ticker.OnTickAction += OnTick;
    }
    protected override void OnTick()
    {
        TryDetectEnemy();
        if(currentDetectedEnemyGO != null & !onReloadTime)
        {
            StartCoroutine(LaunchShell());
        }
    }
    protected override void Update()
    {
       
    }

    private IEnumerator LaunchShell()
    {
        onReloadTime = true;
        Instantiate(projectileGO, shootPoint.position, shootPoint.rotation);
        StartCoroutine(Reload());
        if(currentDetectedEnemyGO != null)
        {
            yield return new WaitForSeconds(timeTillNextExplosion);
            ExplosionAtEnemy(currentDetectedEnemyGO);
        }
    }


    private void ExplosionAtEnemy(GameObject enemy)
    {
        if (enemy == null) { return; }
        Instantiate(explosionGO, enemy.transform.position, enemy.transform.rotation);
        var middleMan = currentDamage * TowerManager.globalTowerDamageMultiplier;
        //enemy.GetComponent<Enemy>().OnHit((int)middleMan), projectileType);
        return;
    }

    protected override IEnumerator TryAttackEnemy()
    {
        yield return null;
    }
}
