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
    public List<GameObject> enemies = new List<GameObject>();
    public static int points;
    public static float pMultiplier = 1;
    public int health;

    public static float enemyHealthMultiplier = 1; 

    public UnityEvent gameOver;
    public UnityEvent gameWon;

    public void Awake()
    {
        instance = this;
    }

    public static void AddPoints(int addedPoints)
    {
        var pointsToAdd = addedPoints * pMultiplier;
        points += (int)pointsToAdd;
    }
    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        if (health < 0)
        {
            gameOver.Invoke();
        }
    }
}
