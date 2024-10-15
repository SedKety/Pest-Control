using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct WaveGroups
{
    public string groupName;
    public int wavePointCost;
    public GameObject[] enemyGO;
    public float timeBetweenEnemy;
    public float timeBetweenGroup;

    public int minWaveEasy;
    public int minWaveMedium;
    public int minWaveHard;
}

[System.Serializable]
public struct Boss
{
    public string bossName;
    public GameObject bossGO;
    public float bossHealthMultiplier;
    public float bossSpawnDelay;
}

public enum GameMode
{
    Easy,
    Medium,
    Hard,
}

public class WaveSystem : MonoBehaviour
{
    public GameMode mode;
    public static WaveSystem instance;
    public static int wave;

    private int wavePoints;
    public int startingWavePoints;
    public float easyWavePointMultiplier;
    public float mediumWavePointMultiplier;
    public float hardWavePointMultiplier;

    public float enemyHealthMultiplierIncrease;


    public List<WaveGroups> enemyGroups;
    public List<WaveGroups> availableGroups;
    public List<WaveGroups> currentWaveGroups;

    public List<Boss> bossEnemies;
    public int bossWaveCount;
    public bool isBossWave;         

    public bool canStartSpawningWaves;
    public Coroutine currentWaveCoroutine;

    public bool isWaveSpawning = false;

    void Awake()
    {
        ResetWaveSystem();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetWaveSystem();
    }

    void ResetWaveSystem()
    {
        instance = this;
        wave = 0;
        wavePoints = 0;
        easyWavePointMultiplier = 1.25f;
        mediumWavePointMultiplier = 1.5f;
        hardWavePointMultiplier = 2f;
        canStartSpawningWaves = false;
        currentWaveCoroutine = null;
    }

    void Start()
    {
        mode = (GameMode)PlayerPrefs.GetInt("Difficulty");
        print(mode.ToString());
        Ticker.OnTickAction += OnTick;
    }

    void OnTick()
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
        isBossWave = wave % bossWaveCount == 0; 

        GameManager.instance.OnWaveChange();
        wavePoints = CalculateWavePoints(wave);

        isWaveSpawning = true;
        currentWaveCoroutine = StartCoroutine(GenerateNewWave());
    }

    int CalculateWavePoints(int wave)
    {
        if (wave <= 0) { wave = 1; }
        float multiplier = mode switch
        {
            GameMode.Easy => easyWavePointMultiplier,
            GameMode.Medium => mediumWavePointMultiplier,
            GameMode.Hard => hardWavePointMultiplier,
            _ => 1f
        };

        return Mathf.RoundToInt(wave * 2f / 3f * multiplier);
    }

    IEnumerator GenerateNewWave()
    {
        UpdateAvailableGroups();

        var maxTries = 10 * wave;
        while (wavePoints > 0 && maxTries >= 0)
        {
            maxTries--;
            currentWaveGroups.Add(SelectGroup());
            UpdateAvailableGroups();
        }

        if (isBossWave)
        {
            currentWaveCoroutine = StartCoroutine(SpawnBoss());
        }

        currentWaveCoroutine = StartCoroutine(SpawnEnemyGroups());
        yield return null;
    }

    WaveGroups SelectGroup()
    {
        var selectedGroup = availableGroups[Random.Range(0, availableGroups.Count)];
        wavePoints -= selectedGroup.wavePointCost;
        return selectedGroup;
    }

    void UpdateAvailableGroups()
    {
        availableGroups.Clear();
        foreach (WaveGroups group in enemyGroups)
        {
            bool isValid = mode switch
            {
                GameMode.Easy => wave >= group.minWaveEasy,
                GameMode.Medium => wave >= group.minWaveMedium,
                GameMode.Hard => wave >= group.minWaveHard,
                _ => false
            };

            if (isValid && group.wavePointCost <= wavePoints)
            {
                availableGroups.Add(group);
            }
        }
    }

    IEnumerator SpawnEnemyGroups()
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
                GameObject e = Instantiate(enemy, GameManager.instance.enemySpawnPos.position, GameManager.instance.enemySpawnPos.rotation);
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

    IEnumerator SpawnBoss()
    {
        Boss selectedBoss = bossEnemies[Random.Range(0, bossEnemies.Count)];

        yield return new WaitForSeconds(selectedBoss.bossSpawnDelay);

        GameObject boss = Instantiate(selectedBoss.bossGO, GameManager.instance.enemySpawnPos.position, GameManager.instance.enemySpawnPos.rotation);
        boss.name += " Boss " + wave;

        GameManager.enemies.Add(boss);
    }

    void OnDestroy()
    {
        instance = null;
    }
}
