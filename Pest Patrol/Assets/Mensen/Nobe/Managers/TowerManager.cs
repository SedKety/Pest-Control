using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public static float globalTowerDamageMultiplier;
    public static float globalTowerAttackSpeedMultiplier;
    public static float globalTowerReloadSpeedMultiplier;
    public static List<Tower> towerList;


    public void Awake()
    {
        instance = this;
    }
    public static void UpdateTowerStats()
    {
        foreach (var tower in towerList)
        {
            var middleMan = 0f;
            middleMan = tower.baseDamage * globalTowerDamageMultiplier;
            tower.currentDamage = (int)middleMan;
            middleMan = tower.baseAttackSpeed * globalTowerAttackSpeedMultiplier;
            tower.currentAttackSpeed = middleMan;
            middleMan = tower.baseReloadSpeed * globalTowerReloadSpeedMultiplier;
            tower.currentReloadSpeed = middleMan;
        }
    }


}
