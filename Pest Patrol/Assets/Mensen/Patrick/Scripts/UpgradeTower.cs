using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTower : MonoBehaviour
{
    public void UpgradeDamage(GameObject tower)
    {
        tower.GetComponent<CombatTower>().baseDamage += 2;
    }
    public void UpgradeRange(GameObject tower)
    {
        tower.GetComponent<CombatTower>().detectionRange += 2;
    }
    public void UpgradeSpeed(GameObject tower)
    {
        tower.GetComponent<CombatTower>().baseReloadSpeed -= 0.1f;
    }
}
