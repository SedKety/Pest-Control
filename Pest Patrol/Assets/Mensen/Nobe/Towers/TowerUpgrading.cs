using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class TowerUpgrading : MonoBehaviour
{
    [Header("GeneralVariables")]
    public Tower currentTowerGO;
    public RawImage towerPreview;
    public TextMeshProUGUI nameText;
    public int maxAllowedUpgrades;

    [Header("GeneratingTowerVariables")]
    public GameObject generatingTowerPanel;
    public TextMeshProUGUI generatedAmountText, generationTimeText;

    [Header("CombatTowerVariables")]
    public GameObject combatTowerPanel;
    public TextMeshProUGUI towerDamageText, towerDamageCostText,
        towerReloadSpeedText, towerReloadSpeedCostText;
    public float damageMultiplierAddedEveryUpgrade;
    public float reloadSpeedMultiplierAddedEveryUpgrade;

    public int standardUpgradeCost;
    public void UpdatePanel(Tower tower)
    {
        currentTowerGO = tower;
        string name = tower.name.Replace("(Clone)", "");
        nameText.text = name;

        GetComponent<UIFoldout>().SetState(true);
        towerPreview.texture = tower.renderTexture;
        switch (tower.typeOfTower)
        {
            case TowerType.generating:
                generatingTowerPanel.gameObject.SetActive(true);
                combatTowerPanel.gameObject.SetActive(false);
                UpdateGeneratingPanel();
                break;
            case TowerType.fighting:

                combatTowerPanel.SetActive(true);
                generatingTowerPanel.gameObject.SetActive(false);
                UpdateCombatPanel();
                break;
            default:
                throw new System.Exception("No valid tower type, if there's a new tower type please add it to the script. I am very bad at programming.");
        }

    }
    public void UpdateCombatPanel()
    {
        CombatTower combatTower = currentTowerGO as CombatTower;
        if (combatTower != null)
        {
            if (combatTower.currentDamageincreasePurchased <= combatTower.maxAllowedDamageIncrease)
            {
                towerDamageText.text = combatTower.baseDamage.ToString();
                towerDamageCostText.text = ((int)(standardUpgradeCost + (combatTower.currentDamageincreasePurchased * 2))).ToString();
            }

            towerReloadSpeedText.text = combatTower.baseReloadSpeed.ToString();
            towerReloadSpeedCostText.text = ((int)(standardUpgradeCost + (combatTower.currentDamageincreasePurchased * 2))).ToString();
        }
        else
        {
            print("Tower aint combat");
        }
    }
    public void UpdateGeneratingPanel()
    {
        GeneratingTower generatingTower = currentTowerGO as GeneratingTower;
        if (generatingTower != null)
        {
            generatedAmountText.text = generatingTower.basePointsGenerated.ToString();
            generationTimeText.text = generatingTower.timeBetweenPoints.ToString();
        }
        else
        {
            print("Tower aint generating");
        }
    }


    public void TryIncreaseTowerDamage()
    {
        CombatTower combatTower = currentTowerGO as CombatTower;
        if (GameManager.points < (int)(combatTower.baseDamage * 2 / 3 + (combatTower.currentDamageincreasePurchased * 2))) { return; }
        else
        {
            combatTower.IncreaseDamage(damageMultiplierAddedEveryUpgrade);
            GameManager.DeletePoints((int)(combatTower.baseDamage * 2 / 3 + (combatTower.currentDamageincreasePurchased * 2)));
        }
    }
    public void TryIncreaseTowerReloadSpeed()
    {
        CombatTower combatTower = currentTowerGO as CombatTower;
        if (GameManager.points < ((int)(combatTower.baseReloadSpeed + (combatTower.currentDamageincreasePurchased * 2)))) { return; }
        else
        {
            GameManager.DeletePoints(((int)(combatTower.baseReloadSpeed + (combatTower.currentDamageincreasePurchased * 2))));
            combatTower.IncreaseReloadSpeed(reloadSpeedMultiplierAddedEveryUpgrade);
            UpdateCombatPanel();
        }
    }
}
