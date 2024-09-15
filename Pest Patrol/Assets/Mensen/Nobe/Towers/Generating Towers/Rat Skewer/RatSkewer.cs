using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSkewer : Tower
{

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

    public void OnDestroy()
    {
        StopCoroutine(GeneratePoints());
    }
}
