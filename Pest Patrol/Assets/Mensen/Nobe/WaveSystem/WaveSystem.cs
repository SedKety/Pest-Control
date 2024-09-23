using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public struct WaveGroups
{
    public string groupName;
    public int wavePointCost;
    public GameObject[] enemyGO;
    public float timeBetweenEnemy;
    public float timeBetweenGroup;
}
public class WaveSystem : MonoBehaviour
{
    public static WaveSystem instance;
    public static int wave;

    //WavePoint variables
    private int wavePoints;
    public int startingWavePoints;
    public float wavePointsModifier;


    // miscelaneus variable
    // miscellaneous
    // wtf is a miscelaneus nobe
    public float enemyHealthMultiplierIncrease;

    //Wave Enemy Variables
    public List<WaveGroups> enemyGroups;

    public List<WaveGroups> availableGroups;
    public List<WaveGroups> currentWaveGroups;

    public Transform enemySpawnPos;

    public bool canStartSpawningWaves;
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
        if (GameManager.enemies.Count == 0 & canStartSpawningWaves)
        {
            StartNewWave();
        }
    }

    [ContextMenu("StartNewWave")]
    public void StartNewWave()
    {
        StopCoroutine(SpawnEnemyGroups());
        currentWaveGroups.Clear();
        GameManager.enemyHealthMultiplier += enemyHealthMultiplierIncrease;
        wave++;
        wavePoints = 0;
        int middleMan = startingWavePoints * wave;
        wavePoints = middleMan;
        StartCoroutine(GenerateNewWave());
    }
    public IEnumerator GenerateNewWave()
    {
        SelectAvailableGroups();

        var maxTries = 10 * wave;
        while (wavePoints > 0 & maxTries >= 0)
        {
            maxTries++;
            currentWaveGroups.Add(SelectGroup());
            SelectAvailableGroups();
        }

        StartCoroutine(SpawnEnemyGroups());
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
            if (groups.wavePointCost <= wavePoints)
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
        for (int i = 0; i  < groups.Count; i++) 
        {
            foreach (GameObject enemy in groups[i].enemyGO)
            {
                GameManager.enemies.Add(Instantiate(enemy, enemySpawnPos.position, enemySpawnPos.rotation));
                yield return new WaitForSeconds(groups[i].timeBetweenEnemy);
            }
            yield return new WaitForSeconds(groups[i].timeBetweenGroup);
        }
        groups.Clear();
        yield return null;
    }
}
