using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/Tower")]
public class TowerSO : ScriptableObject
{
    public GameObject towerGhostGO, towerToPlaceGO;
    public Sprite towerSprite;
    public int towerBuildPointCost;
}
