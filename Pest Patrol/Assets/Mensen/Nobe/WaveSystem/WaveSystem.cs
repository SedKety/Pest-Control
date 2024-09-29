using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public struct WaveGroups
{
    public string groupName;
    public int wavePointCost;
    public GameObject[] enemyGO;
    public float timeBetweenEnemy;
    public float timeBetweenGroup;

    public int minWave;
}

public class WaveSystem : MonoBehaviour
{

    public static WaveSystem instance;
    public static int wave;

    //WavePoint variables
    private int wavePoints;
    public int startingWavePoints;
    public float wavePointsModifier;

    // Miscellaneous variables
    public float enemyHealthMultiplierIncrease;

    //Wave Enemy Variables
    public List<WaveGroups> enemyGroups;
    public List<WaveGroups> availableGroups;
    public List<WaveGroups> currentWaveGroups;

    public bool canStartSpawningWaves;
    public Coroutine currentWaveCoroutine;

    public bool isWaveSpawning = false;

    public void Awake()
    {
        ResetWaveSystem();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetWaveSystem();
    }
    public void ResetWaveSystem()
    {
        instance = this;
        wave = 0;
        wavePoints = 0;
        wavePointsModifier = 1;
        canStartSpawningWaves = false;
        currentWaveCoroutine = null;
    }
    public void Start()
    {
        Ticker.OnTickAction += OnTick;
    }
    public void OnTick()
    {
        if (GameManager.enemies.Count == 0 && canStartSpawningWaves && !isWaveSpawning)
        {
            StartNewWave();
        }
    }

    [ContextMenu("StartNewWave")]
    public void StartNewWave()
    {
        if (isWaveSpawning && currentWaveCoroutine != null)
        {
            StopCoroutine(currentWaveCoroutine); 
            currentWaveCoroutine = null;        
        }

        currentWaveGroups.Clear();
        GameManager.enemyHealthMultiplier += enemyHealthMultiplierIncrease;

        wave++;
        GameManager.instance.OnWaveChange();
        wavePoints = 0;
        int middleMan = startingWavePoints * wave;
        wavePoints = middleMan;

        isWaveSpawning = true; 
        currentWaveCoroutine = StartCoroutine(GenerateNewWave());
    }

    public IEnumerator GenerateNewWave()
    {
        SelectAvailableGroups();

        var maxTries = 10 * wave;
        while (wavePoints > 0 && maxTries >= 0)
        {
            maxTries--;
            currentWaveGroups.Add(SelectGroup());
            SelectAvailableGroups();
        }

        currentWaveCoroutine = StartCoroutine(SpawnEnemyGroups());
        yield return null;
    }

    public WaveGroups SelectGroup()
    {
        var selectedGroup = availableGroups[Random.Range(0, availableGroups.Count)];
        wavePoints -= selectedGroup.wavePointCost;
        return selectedGroup;
    }

    public void SelectAvailableGroups()
    {
        availableGroups.Clear();
        foreach (WaveGroups groups in enemyGroups)
        {
            if (groups.wavePointCost <= wavePoints & wave >= groups.minWave)
            {
                availableGroups.Add(groups);
            }
        }
    }

    public IEnumerator SpawnEnemyGroups()
    {
        List<WaveGroups> groups = new List<WaveGroups>();
        foreach (WaveGroups group in currentWaveGroups)
        {
            groups.Add(group);
        }

        currentWaveGroups.Clear();
        for (int i = 0; i < groups.Count; i++)
        {
            foreach (GameObject enemy in groups[i].enemyGO)
            {
                GameObject e = Instantiate(enemy, GameManager.wayPoints[0].position, GameManager.wayPoints[0].rotation);
                GameManager.enemies.Add(e);
                e.name += wave;
                yield return new WaitForSeconds(groups[i].timeBetweenEnemy);
            }
            yield return new WaitForSeconds(groups[i].timeBetweenGroup);
        }

        groups.Clear();
        yield return null;

        isWaveSpawning = false;
    }

    public void OnDestroy()
    {
        instance = null;
    }
}