using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
    public static List<Transform> flyingWaypoints = new List<Transform>();
    public List<Transform> flyingWavePoints = new List<Transform>();


    public List<Transform> waypointsToAddAsLast = new List<Transform>();
    public List<Transform> flyingWaypointsToAddAsLast = new List<Transform>();
    public static List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> enemieTest = new List<GameObject>();
    public static long points;
    public static float pMultiplier = 1;
    public static float enemyHealthMultiplier = 1;


    public static bool GameHasStarted;

    public int health;
    public int startingPoints;
    public int maxWave;

    public UnityEvent gameOver;
    public UnityEvent gameWon;

    public static GamePhase gamePhase;

    public static int tickCount;

    public TextMeshProUGUI pointDisplay;

    public Transform enemySpawnPos;

    public GameObject waypointFlag, endPointFlag;
    public bool shouldSpawnFlags;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    public void OnWaveChange()
    {
        print("currentWave = " + WaveSystem.wave);
        if (WaveSystem.wave > maxWave)
        {
            gameWon.Invoke();
        }
    }

    public void Start()
    {
        Time.timeScale = 1;
        points = 0;
        GameHasStarted = false;
        wayPoints.Clear();
        enemies.Clear();
        tickCount = 0;
        enemyHealthMultiplier = 1;
        Ticker.OnTickAction += OnTick;
        AddPoints(startingPoints);
        flyingWavePoints.Clear();
        flyingWaypoints.Clear();
        if (PlayerPrefs.GetInt("EndlessMode") is 1) //makeshift bool yippee
        {
            maxWave = 9999999;
        }
    }

    public void SkipWave()
    {
        WaveSystem.instance.StartNewWave();
    }
    public void OnTick()
    {
        tickCount++;
        pointDisplay.text = points.ToString();
    }
    public void Update()
    {
        wavePoints = wayPoints;
        enemieTest = enemies;
        flyingWavePoints = flyingWaypoints;
    }
    public static void AddPoints(long addedPoints)
    {
        var pointsToAdd = addedPoints * pMultiplier;
        points += (int)pointsToAdd;
    }
    public static void DeletePoints(long removedPoints)
    {
        var pointsToRemove = removedPoints;
        points -= (int)pointsToRemove;
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
        WaveSystem.instance.isIngame = true;
        BuildingSystem.Instance.EnterTowerPhase();
        AddPoints(startingPoints);
        for (int i = 0; i < waypointsToAddAsLast.Count; i++)
        {
            wayPoints.Add(waypointsToAddAsLast[i]);
        }
        for (int i = 0; i < flyingWaypointsToAddAsLast.Count; i++)
        {
            flyingWaypoints.Add(flyingWaypointsToAddAsLast[i]);
        }

        if (shouldSpawnFlags)
        {
            SpawnFlags();
        }
    }

    public void SpawnFlags()
    {
        for (int i = 0; i < wayPoints.Count; i++)
        {
            if (wayPoints.Count > i + 1)
            {
                Instantiate(waypointFlag, wayPoints[i].position, Quaternion.identity);
            }
            else if (wayPoints.Count <= i + 1)
            {
                Instantiate(endPointFlag, wayPoints[i].position, Quaternion.identity);
            }
        }
    }
    public void OnDestroy()
    {
        instance = null;
    }
    [ContextMenu("AddPointsDevTool")]
    public void AddPointsDevTool()
    {
        AddPoints(1000000);
    }

    public void TimeScale0()
    {
        Time.timeScale = 0;
    }

    public void StopWave()
    {
        WaveSystem.instance.StopWave();
    }
}
