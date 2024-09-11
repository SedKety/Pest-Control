using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //globally accessed variables
    public List<Transform> wayPoints = new List<Transform>();
    public List<Enemy> enemies = new List<Enemy>();
    public int points;
    public float pMultiplier = 1;
    public int health;

    public UnityEvent gameOver;

    public void Awake()
    {
        instance = this;
    }

    public void AddPoints(int addedPoints)
    {
        var pointsToAdd = addedPoints * pMultiplier;
        points += (int)pointsToAdd;
        print("Points");
    }
    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        if (health < 0)
        {
            gameOver.Invoke();
        }
    }
    public async Task StunAllEnemies(float timeInSeconds)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.cantMove = true;
        }
        var middleMan = timeInSeconds * 1000;
        print((int)middleMan);
        await Task.Delay((int)middleMan);
        foreach (Enemy enemy in enemies)
        {
            enemy.cantMove = false;
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        AddPoints(enemy.pointsDropped);
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
