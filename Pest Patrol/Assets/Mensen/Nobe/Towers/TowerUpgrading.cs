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

    public int damageUpgradeCostMultiplier;
    public int reloadSpeedUpgradeCostMultiplier;

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
            case TowerType.fighting:
                combatTowerPanel.SetActive(true);
                UpdateCombatPanel();
                break;
            default:
                throw new System.Exception("No valid tower type. If there is a new tower type, please add it to the script.");
        }
    }

    public void UpdateCombatPanel()
    {
        CombatTower combatTower = currentTowerGO as CombatTower;
        if (combatTower != null)
        {
            if (combatTower.currentDamageincreasePurchased + 1 <= combatTower.maxAllowedDamageIncrease)
            {
                towerDamageCostText.text = ((int)(standardUpgradeCost + (combatTower.currentDamageincreasePurchased * damageUpgradeCostMultiplier))).ToString();
                print(combatTower.currentDamageincreasePurchased + " " + combatTower.maxAllowedDamageIncrease);
            }
            else
            {
                towerDamageCostText.text = "MAX";
            }

            if (combatTower.currentReloadSpeedincreasePurchased + 1 <= combatTower.maxAllowedReloadSpeed)
            {
                towerReloadSpeedCostText.text = ((int)(standardUpgradeCost + (combatTower.currentReloadSpeedincreasePurchased * reloadSpeedUpgradeCostMultiplier))).ToString();
            }
            else
            {
                towerReloadSpeedCostText.text = "MAX";
            }

            towerDamageText.text = combatTower.currentDamage.ToString();
            towerReloadSpeedText.text = Math.Round(combatTower.currentReloadSpeed, 2).ToString();
        }
        else
        {
            Debug.LogError("Current tower is not a combat tower.");
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
            Debug.LogError("Current tower is not a generating tower.");
        }
    }

    public void TryIncreaseTowerDamage()
    {
        CombatTower combatTower = currentTowerGO as CombatTower;
        if (combatTower == null) return;  

        int upgradeCost = (int)(standardUpgradeCost + (combatTower.currentDamageincreasePurchased * damageUpgradeCostMultiplier));

        if (GameManager.points < upgradeCost || combatTower.currentDamageincreasePurchased >= combatTower.maxAllowedDamageIncrease)
        {
            return;  
        }

        combatTower.IncreaseDamage(damageMultiplierAddedEveryUpgrade);
        GameManager.DeletePoints(upgradeCost);
        UpdateCombatPanel();  
    }

    public void TryIncreaseTowerReloadSpeed()
    {
        CombatTower combatTower = currentTowerGO as CombatTower;
        if (combatTower == null) return;  

        int upgradeCost = (int)(standardUpgradeCost + (combatTower.currentReloadSpeedincreasePurchased * reloadSpeedUpgradeCostMultiplier));

        if (GameManager.points < upgradeCost || combatTower.currentReloadSpeedincreasePurchased >= combatTower.maxAllowedReloadSpeed)
        {
            return;  
        }

        GameManager.DeletePoints(upgradeCost);
        combatTower.IncreaseReloadSpeed(reloadSpeedMultiplierAddedEveryUpgrade);
        UpdateCombatPanel(); 
    }
}
