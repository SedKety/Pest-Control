using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrradiatedRat : Enemy
{
    public float minionInterval;
    public int minionToSpawnCount;
    public GameObject minionGO;


    public int healAmount;
    public float healInterval;
    public int maxRatsSacrificed;

    public override void Start()
    {
        FaceWaypoint();
        StartCoroutine(SpawnMinions());
        health *= (int)(WaveSystem.wave * 0.1f);
    }

    public IEnumerator SpawnMinions()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(minionInterval);
            for (int i = 0; i < minionToSpawnCount; i++)
            {
                var enemy = Instantiate(minionGO, transform.position, transform.rotation).GetComponent<Enemy>();
                enemy.currentWaypoint = currentWaypoint;
            }
        }
    }
}
