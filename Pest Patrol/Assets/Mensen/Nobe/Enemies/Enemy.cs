using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    flying,
    walking,
    tank
}
public abstract class Enemy : MonoBehaviour
{
    //enemy info
    public int health;
    public int damage;
    public EnemyType type;

    //movement variables
    public bool cantMove;
    public float moveSpeed;
    public float minDistance;
    private int currentWaypoint = 0;

    //miscelaneous variables
    public int pointsDropped;

    public virtual void Start()
    {
        var middleMan = health * GameManager.enemyHealthMultiplier;
        health = (int)middleMan;
    }
    public virtual void Update()
    {
        MoveTowardsWaypoint();
    }

    public virtual void MoveTowardsWaypoint()
    {
        if (cantMove) return;
        var step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.wayPoints[currentWaypoint].position, step);
        var distance = Vector3.Distance(transform.position, GameManager.instance.wayPoints[currentWaypoint].position);
        if (distance < minDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= GameManager.instance.wayPoints.Count)
            {
                GameManager.instance.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    public virtual void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.RegisterEnemyDeath(this);
        }
    }
}
