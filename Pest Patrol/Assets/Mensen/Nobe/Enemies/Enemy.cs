using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public enum EnemyType
{
    flying,
    walking,
    all,
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
    protected int currentWaypoint = 0;

    //miscelaneous variables
    public int pointsDropped;


    public bool isDead;

    public virtual void Start()
    {
        var middleMan = health * GameManager.enemyHealthMultiplier;
        health = (int)middleMan;
        FaceWaypoint();
    }
    public virtual void Update()
    {
        MoveTowardsWaypoint();
    }
    public virtual void MoveTowardsWaypoint()
    {
        if (cantMove) return;
        CheckIfAtEnd();
        var step = moveSpeed * Time.deltaTime;
        if (transform == null) { return; }
        if (GameManager.wayPoints[currentWaypoint])
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.wayPoints[currentWaypoint].position, step);
            var distance = Vector3.Distance(transform.position, GameManager.wayPoints[currentWaypoint].position);
            if (distance < minDistance)
            {
                currentWaypoint++;
                FaceWaypoint();
                CheckIfAtEnd();
            }
        }
        else if (!GameManager.wayPoints[currentWaypoint])
        {
            CheckIfAtEnd();
        }
    }

    public virtual void CheckIfAtEnd()
    {
        if (currentWaypoint >= GameManager.wayPoints.Count)
        {
            GameManager.instance.TakeDamage(damage);
            isDead = true;
            Destroy(gameObject);
        }
    }
    public virtual void FaceWaypoint()
    {
        CheckIfAtEnd();
        if(isDead) return;
        float distance = Vector3.Distance(transform.position, GameManager.wayPoints[currentWaypoint].position);
        bool nextWaypointActive = CheckIfNextWaypointExists();
        if (distance < minDistance * 2 & nextWaypointActive)
        {
            transform.LookAt(GameManager.wayPoints[currentWaypoint + 1].position);
        }
        else
        {
            transform.LookAt(GameManager.wayPoints[currentWaypoint].position);
        }
    }

    public virtual bool CheckIfNextWaypointExists()
    {
        if(GameManager.wayPoints.Count >= currentWaypoint + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void OnHit(int damage, ProjectileType projectileType)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        GameManager.AddPoints(pointsDropped);
        GameManager.enemies.Remove(gameObject);
    }
}
