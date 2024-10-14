using System;
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
    public UIFoldout uiFoldout;
    public int standardUpgradeCost;

    private void Start()
    {
        uiFoldout = GetComponent<UIFoldout>();
        uiFoldout.interactable = false;
    }

    public void UpdatePanel(Tower tower)
    {
        currentTowerGO = tower;
        string name = tower.name.Replace("(Clone)", "");
        nameText.text = name;

        uiFoldout.SetState(true);
        towerPreview.texture = tower.renderTexture;
        switch (tower.typeOfTower)
        {
            case TowerType.generating:
                generatingTowerPanel.gameObject.SetActive(true);
                UpdateGeneratingPanel();
                break;
            case TowerType.fighting:

                combatTowerPanel.SetActive(true);
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
                towerDamageCostText.text = ((int)(standardUpgradeCost + (combatTower.currentDamageincreasePurchased * 2))).ToString();
            }
            else
            {
                towerDamageCostText.text = "MAX";
            }
            if(combatTower.currentReloadSpeedincreasePurchased <= combatTower.maxAllowedReloadSpeed)
            {
                towerReloadSpeedCostText.text = ((int)(standardUpgradeCost + (combatTower.currentReloadSpeedincreasePurchased * 2))).ToString();
            }
            else
            {
                towerReloadSpeedCostText.text = "MAX";
            }
            towerDamageText.text = combatTower.currentDamage.ToString();
            towerReloadSpeedText.text = combatTower.currentReloadSpeed.ToString();
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
        if (GameManager.points < (int)(combatTower.currentDamage * 2 / 3 + (combatTower.currentDamageincreasePurchased * 2)) || combatTower.currentDamageincreasePurchased > combatTower.maxAllowedDamageIncrease) { return; }
        else
        {
            combatTower.IncreaseDamage(damageMultiplierAddedEveryUpgrade);
            GameManager.DeletePoints((int)(combatTower.currentDamage * 2 / 3 + (combatTower.currentDamageincreasePurchased * 2)));
            UpdateCombatPanel();
        }
    }
    public void TryIncreaseTowerReloadSpeed()
    {
        
        CombatTower combatTower = currentTowerGO as CombatTower;
        if (GameManager.points < ((int)(combatTower.baseReloadSpeed + (combatTower.currentReloadSpeedincreasePurchased * 2))) || combatTower.currentReloadSpeedincreasePurchased > combatTower.maxAllowedReloadSpeed) { return; }
        else
        {
            GameManager.DeletePoints(((int)(combatTower.baseReloadSpeed + (combatTower.currentReloadSpeedincreasePurchased * 2))));
            combatTower.IncreaseReloadSpeed(reloadSpeedMultiplierAddedEveryUpgrade);
            UpdateCombatPanel();
        }
    }
}
