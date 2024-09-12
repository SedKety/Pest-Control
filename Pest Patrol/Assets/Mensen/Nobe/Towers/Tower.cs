using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargettingType
{
    ground,
    walking,
    all,
    none,
}

public enum TowerType
{
    generating,
    fighting,
}
public abstract class Tower : MonoBehaviour
{
    public int towerPointCost;
    public TargettingType typeToTarget;
    public TowerType typeOfTower;
    //base Stats, these will be multiplied by the towermanager class
    [Header("Attacking Tower Variables")]
    public int baseDamage;
    public float baseAttackSpeed;
    public float baseReloadSpeed;

    [Header("Generating Tower Variables")]
    public int basePointsGenerated;
    public float timeBetweenPoints;

    //Multiplied Variables
    [HideInInspector] public int currentDamage;
    [HideInInspector] public float currentAttackSpeed;
    [HideInInspector] public float currentReloadSpeed;



}
