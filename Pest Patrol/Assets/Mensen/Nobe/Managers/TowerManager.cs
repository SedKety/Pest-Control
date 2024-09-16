using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public static float globalTowerDamageMultiplier = 1;
    [Tooltip("Dont increase this, instead decrease it.")]
    public static float globalTowerReloadSpeedMultiplier = 1;
    public static float globalProjectileMovementSpeedMultiplier = 1;

    public static List<Tower> towerList;


    public void Awake()
    {
        instance = this;
    }

}
