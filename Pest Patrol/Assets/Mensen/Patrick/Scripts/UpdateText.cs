using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class UpdateText : MonoBehaviour
{
    public TMP_Text[] coinTexts;
    public TMP_Text nameText;
    public RawImage skibidi;
    public static UpdateText instance;
    public GameObject[] generatingTowerUpgrades;
    public GameObject[] combatTowerUpgrades;
    public GameObject currentTower;
    public UIFoldout uiFoldout;
    private void Start()
    {
        instance = this;
        uiFoldout = GetComponent<UIFoldout>();
        uiFoldout.interactable = false;
    }

    void UpdateValues()
    {
        coinTexts[0].text = Mathf.RoundToInt((currentTower.gameObject.GetComponent<CombatTower>().currentDamage * 2) * 1.2f).ToString();
        coinTexts[1].text = Mathf.RoundToInt((currentTower.gameObject.GetComponent<CombatTower>().detectionRange * 2) * 1.2f).ToString();

        coinTexts[3].text = currentTower.gameObject.GetComponent<CombatTower>().currentDamage.ToString();
        coinTexts[4].text = currentTower.gameObject.GetComponent<CombatTower>().detectionRange.ToString();
        if (currentTower.gameObject.GetComponent<CombatTower>().baseReloadSpeed > 0.1f)
        {
            coinTexts[2].text = Mathf.RoundToInt((currentTower.gameObject.GetComponent<CombatTower>().baseReloadSpeed + (1 - currentTower.gameObject.GetComponent<CombatTower>().baseReloadSpeed * 6) + 15 * 1.2f)).ToString();
            coinTexts[5].text = (Mathf.Round((currentTower.gameObject.GetComponent<CombatTower>().baseReloadSpeed * 10.0f)) * 0.1f).ToString();
        }
    }
    public void UpdatePanel(Tower tower)
    {
        var name = tower.name.Replace("(Clone)", "");
        nameText.text = name;
        uiFoldout.SetState(true);
        skibidi.texture = tower.renderTexture;
        switch (tower.typeOfTower)
        {
            case TowerType.generating:
                foreach (var go in generatingTowerUpgrades) go.SetActive(true);
                break;
            case TowerType.fighting:
                foreach (var go in combatTowerUpgrades) go.SetActive(true);
                break;
            default:
                throw new System.Exception("No valid tower type, if there's a new tower type please add it to the script. I am very bad at programming.");
        }
        //0 = damage coins & 3 = damage count
        //1 = range coins & 4 = range count
        //2 = speed coins & 5 = speed count
        coinTexts[0].text = Mathf.RoundToInt((tower.gameObject.GetComponent<CombatTower>().currentDamage * 2) * 1.2f).ToString();
        coinTexts[1].text = Mathf.RoundToInt((tower.gameObject.GetComponent<CombatTower>().detectionRange * 2) * 1.2f).ToString();
        
        coinTexts[3].text = tower.gameObject.GetComponent<CombatTower>().currentDamage.ToString();
        coinTexts[4].text = tower.gameObject.GetComponent<CombatTower>().detectionRange.ToString();
        if (tower.gameObject.GetComponent<CombatTower>().baseReloadSpeed > 0.1f)
        {
            coinTexts[2].text = Mathf.RoundToInt((tower.GetComponent<CombatTower>().baseReloadSpeed + 15 * 1.5f)).ToString();
            coinTexts[5].text = (Mathf.Round((tower.gameObject.GetComponent<CombatTower>().baseReloadSpeed * 10.0f)) * 0.1f).ToString();
        }
        currentTower = tower.gameObject;
    }

    public void UpgradeDamage()
    {
        var cost = Mathf.RoundToInt((currentTower.GetComponent<CombatTower>().currentDamage * 2) * 1.2f);
        if (GameManager.points - cost < 0) return;
        currentTower.GetComponent<CombatTower>().currentDamage += 2;
        GameManager.DeletePoints(cost);
        UpdateValues();
    }
    public void UpgradeRange()
    {
        var cost = Mathf.RoundToInt((currentTower.GetComponent<CombatTower>().detectionRange * 2) * 1.2f);
        if (GameManager.points - cost < 0) return;
        currentTower.GetComponent<CombatTower>().detectionRange += 2;
        GameManager.DeletePoints(cost);
        UpdateValues();
    }
    public void UpgradeSpeed()
    {
        var cost = Mathf.RoundToInt((currentTower.GetComponent<CombatTower>().baseReloadSpeed + 15 * 1.5f));
        if (GameManager.points - cost < 0) return;
        if (currentTower.GetComponent<CombatTower>().baseReloadSpeed - 0.1f > 0)
        {
            currentTower.GetComponent<CombatTower>().baseReloadSpeed -= 0.1f;
            GameManager.DeletePoints(cost);
            UpdateValues();
        }
        else
        {
            coinTexts[5].text = "MAX";
        }
    }
}
