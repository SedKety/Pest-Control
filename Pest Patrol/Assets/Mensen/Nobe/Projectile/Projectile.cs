using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int tickLifeTime;
    protected int currentTickCount;
    public int movementSpeed;
    public GameObject enemyGO;
    [HideInInspector]
    public int projectileDamage;
    public int pierceAmount;
    protected int pierceCount;

    public List<GameObject> hitEnemy;
    public float hitDistance;
    protected virtual void Start()
    {
        Ticker.OnTickAction += OnTick;
    }
    protected virtual void Update()
    {

    }
    protected virtual void OnTick()
    {
        currentTickCount++;
        if (currentTickCount >= tickLifeTime)
        {
            Ticker.OnTickAction -= OnTick;
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
    public void OnDestroy()
    {
        Ticker.OnTickAction -= OnTick;
    }
}
