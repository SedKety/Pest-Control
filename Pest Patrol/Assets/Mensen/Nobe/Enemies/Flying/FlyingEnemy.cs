using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public override void Start()
    {
        base.Start();
        transform.position += new Vector3(0, 2f, 0);
    }
    public override void MoveTowardsWaypoint()
    {
        if (cantMove) return;
        CheckIfAtEnd();
        var step = moveSpeed * Time.deltaTime;
        if (transform == null) { return; }
        if (GameManager.flyingWaypoints[currentWaypoint])
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.flyingWaypoints[currentWaypoint].position, step);
            var distance = Vector3.Distance(transform.position, GameManager.flyingWaypoints[currentWaypoint].position);
            if (distance < minDistance)
            {
                currentWaypoint++;
                FaceWaypoint();
                CheckIfAtEnd();
            }
        }
        else if (!GameManager.flyingWaypoints[currentWaypoint])
        {
            CheckIfAtEnd();
        }
    }
    public override void FaceWaypoint()
    {
        CheckIfAtEnd();
        if (isDead || gameObject == null || transform == null || GameManager.flyingWaypoints[currentWaypoint] == null) return;
        float distance = Vector3.Distance(transform.position, GameManager.flyingWaypoints[currentWaypoint].position);
        bool nextWaypointActive = CheckIfNextWaypointExists();
        if (distance < minDistance * 2 & nextWaypointActive)
        {
            transform.LookAt(GameManager.flyingWaypoints[currentWaypoint + 1].position);
        }
        else
        {
            transform.LookAt(GameManager.flyingWaypoints[currentWaypoint].position);
        }
    }

    public override bool CheckIfNextWaypointExists()
    {
        if (GameManager.flyingWaypoints.Count >= currentWaypoint + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void CheckIfAtEnd()
    {
        if (currentWaypoint >= GameManager.flyingWaypoints.Count)
        {
            GameManager.instance.TakeDamage(damage);
            isDead = true;
            Destroy(gameObject);
        }
    }
}
