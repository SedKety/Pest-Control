using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeTower : MonoBehaviour
{
    public TMP_Text[] coinTexts;
    private void Start()
    {
        Ticker.OnTickAction += OnTick;
    }

    public void OnTick()
    {
        coinTexts[0].text = Mathf.RoundToInt((gameObject.GetComponent<CombatTower>().baseDamage * 2) * 1.2f).ToString();
        coinTexts[1].text = Mathf.RoundToInt((gameObject.GetComponent<CombatTower>().detectionRange * 2) * 1.2f).ToString();
        coinTexts[2].text = Mathf.RoundToInt((gameObject.GetComponent<CombatTower>().baseReloadSpeed + 15 * 1.2f)).ToString();
    }
    public void UpgradeDamage(GameObject tower)
    {
        int cost = Mathf.RoundToInt((tower.GetComponent<CombatTower>().baseDamage * 2) * 1.2f);
        if (GameManager.points - cost >= 0)
        {
            tower.GetComponent<CombatTower>().baseDamage += 2;
            GameManager.DeletePoints(cost);
        }
    }
    public void UpgradeRange(GameObject tower)
    {
        int cost = Mathf.RoundToInt((tower.GetComponent<CombatTower>().detectionRange * 2) * 1.2f);
        if (GameManager.points - cost >= 0)
        {
            tower.GetComponent<CombatTower>().detectionRange += 2;
            GameManager.DeletePoints(cost);
        }
    }
    public void UpgradeSpeed(GameObject tower)
    {
        int cost = Mathf.RoundToInt((tower.GetComponent<CombatTower>().baseReloadSpeed + 15 * 1.2f));
        if (GameManager.points - cost >= 0)
        {
            tower.GetComponent<CombatTower>().baseReloadSpeed -= 0.1f;
            GameManager.DeletePoints(cost);
        }
    }
}