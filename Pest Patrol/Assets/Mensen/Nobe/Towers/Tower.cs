using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum TargettingType
{
    ground,
    walking,
    all,
    none,
}

public enum TowerType
{
    generating,
    fighting,
}
public abstract class Tower : MonoBehaviour
{
    public int towerPointCost;
    public TargettingType typeToTarget;
    public TowerType typeOfTower;
    public bool canShoot = true;
    //base Stats, these will be multiplied by the towermanager class
    [Header("Attacking Tower Variables")]
    public int baseDamage;
    public float baseAttackSpeed;
    public float baseReloadSpeed;
    public float detectionRange;
    public GameObject currentDetectedEnemy;
    [Header("Generating Tower Variables")]
    public int basePointsGenerated;
    public float timeBetweenPoints;

    //Multiplied Variables
    [HideInInspector] public int currentDamage;
    [HideInInspector] public float currentAttackSpeed;
    [HideInInspector] public float currentReloadSpeed;

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }

    protected virtual void OnTick()
    {

    }
    public async Task StunTower(float timeInSeconds)
    {
        canShoot = false;
        var middleMan = timeInSeconds * 1000;
        print((int)middleMan);
        await Task.Delay((int)middleMan);
        canShoot = true;
    }
    protected virtual void EnemyDistanceCheck()
    {

    }
    protected virtual void TryDetectEnemy()
    {
        if(currentDetectedEnemy) { return; } 
        var distance = 0f;
        foreach (GameObject enemy in GameManager.instance.enemies)
        {
            if(enemy != null)
            {
                distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= detectionRange)
                {
                    currentDetectedEnemy = enemy;
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
