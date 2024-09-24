using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
public enum GamePhase
{
    mapBuildPhase,
    wavePhase,
    winPhase,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //globally accessed variables
    public static List<Transform> wayPoints = new List<Transform>();
    public List<Transform> wavePoints = new List<Transform>();
    public static List<GameObject> enemies = new List<GameObject>();
    public static long points;
    public static float pMultiplier = 1;
    public static float enemyHealthMultiplier = 1;

    public static bool GameHasStarted;

    public int health;

    public int maxWave;

    public UnityEvent gameOver;
    public UnityEvent gameWon;

    public static GamePhase gamePhase;

    public static int tickCount;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        Ticker.OnTickAction += OnTick;
    }
    public void OnTick()
    {
        tickCount++;
    }
    public void Update()
    {
        wavePoints = wayPoints;
    }
    public static void AddPoints(long addedPoints)
    {
        var pointsToAdd = addedPoints * pMultiplier;
        points += (int)pointsToAdd;
    }
    public static void RemovePoints(long removedPoints)
    {
        points -= (long)removedPoints;
    }
    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            gameOver.Invoke();
        }
    }
    [ContextMenu("EnterWavePhase")]
    public void EnterWavePhase()
    {
        gamePhase = GamePhase.wavePhase;
        WaveSystem.instance.canStartSpawningWaves = true;
        BuildingSystem.Instance.EnterTowerPhase();
    }
}
