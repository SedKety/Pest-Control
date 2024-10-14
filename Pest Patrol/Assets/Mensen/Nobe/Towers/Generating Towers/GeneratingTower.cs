using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratingTower : Tower
{
    [Header("Generating Tower Variables")]
    public int basePointsGenerated;
    private float basePointMultiplier = 1;

    public float timeBetweenPoints;
    private float timeBetweenPointsMultiplier = 1;

    protected override void Start()
    {
        StartCoroutine(GeneratePoints());
    }
    public IEnumerator GeneratePoints()
    {
        yield return new WaitForSeconds(timeBetweenPoints);
        GameManager.AddPoints(basePointsGenerated);
        print(basePointsGenerated);
        StartCoroutine(GeneratePoints());
    }

    public override void OnDestroy()
    {
        StopCoroutine(GeneratePoints());
    }
}
