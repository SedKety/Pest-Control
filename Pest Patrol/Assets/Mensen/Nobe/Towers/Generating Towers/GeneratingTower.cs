using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratingTower : Tower
{
    [Header("Generating Tower Variables")]
    public int basePointsGenerated;
    public float timeBetweenPoints;

    protected override void Start()
    {
        StartCoroutine(GeneratePoints());
    }
    public IEnumerator GeneratePoints()
    {
        yield return new WaitForSeconds(timeBetweenPoints);
        GameManager.AddPoints(basePointsGenerated);
        StartCoroutine(GeneratePoints());
    }

    public override void OnDestroy()
    {
        StopCoroutine(GeneratePoints());
    }
}
