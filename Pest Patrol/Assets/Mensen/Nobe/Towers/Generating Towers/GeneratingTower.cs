using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratingTower : Tower
{
    [Header("Generating Tower Variables")]
    public int basePointsGenerated;
    public int maxPointsGenerated;
    private float basePointMultiplier = 1;

    public float timeBetweenPoints;
    public float minTimeBetweenPoints;
    private float timeBetweenPointsMultiplier = 1;


    public void IncreasePointsGenerated(float pointMultiplier)
    {
        basePointMultiplier += pointMultiplier;
        basePointsGenerated = (int)(basePointsGenerated * basePointMultiplier);
    }

    public void IncreaseTimeBetweenPoints(float timeBetweenMultiplier)
    {

    }
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
