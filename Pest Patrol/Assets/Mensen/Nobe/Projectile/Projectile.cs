using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int tickLifeTime;
    protected int currentTickCount;
    public int movementSpeed;

    public GameObject enemyGO;
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
            Destroy(gameObject);
        }
    }
}
