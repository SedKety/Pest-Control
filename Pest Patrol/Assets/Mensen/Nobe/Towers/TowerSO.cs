using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Tower")]
public class TowerSO : ScriptableObject
{
    public GameObject towerGhostGO, towerToPlaceGO;
    public Sprite towerSprite;
    public int towerBuildPointCost;

    public int allowedOverlaps;
}
