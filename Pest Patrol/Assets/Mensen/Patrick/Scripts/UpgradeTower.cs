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
        coinTexts[0].text = Mathf.RoundToInt((gameObject.GetComponent<CombatTower>().currentDamage * 2) * 1.2f).ToString();
        coinTexts[1].text = Mathf.RoundToInt((gameObject.GetComponent<CombatTower>().detectionRange * 2) * 1.2f).ToString();
        coinTexts[2].text = Mathf.RoundToInt((gameObject.GetComponent<CombatTower>().baseReloadSpeed + (1 - gameObject.GetComponent<CombatTower>().baseReloadSpeed * 6) + 15 * 1.2f)).ToString();
        coinTexts[3].text = gameObject.GetComponent<CombatTower>().currentDamage.ToString();
        coinTexts[4].text = gameObject.GetComponent<CombatTower>().detectionRange.ToString();
        if (gameObject.GetComponent<CombatTower>().baseReloadSpeed > 0.1f)
        {
            coinTexts[5].text = (Mathf.Round((gameObject.GetComponent<CombatTower>().baseReloadSpeed * 10.0f)) * 0.1f).ToString();

        }
    }
    public void UpgradeDamage(GameObject tower)
    {
        int cost = Mathf.RoundToInt((tower.GetComponent<CombatTower>().currentDamage * 2) * 1.2f);
        if (GameManager.points - cost >= 0)
        {
            tower.GetComponent<CombatTower>().currentDamage += 2;
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
        int cost = Mathf.RoundToInt((tower.GetComponent<CombatTower>().baseReloadSpeed + 15 * 1.5f));
        if (GameManager.points - cost >= 0)
        {
            if (tower.GetComponent<CombatTower>().baseReloadSpeed - 0.1f > 0)
            {
                tower.GetComponent<CombatTower>().baseReloadSpeed -= 0.1f;
                GameManager.DeletePoints(cost);
            }
            else
            {
                coinTexts[5].text = "MAX";
            }
        }
    }

    public void DestroyTower(GameObject tower)
    {
        BuildingSystem.Instance.DeleteTower(tower);
    }

    public void OnDestroy()
    {
        Ticker.OnTickAction -= OnTick;
    }
}
