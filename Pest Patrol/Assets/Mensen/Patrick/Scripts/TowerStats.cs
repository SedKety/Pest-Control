using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
[System.Serializable]
public struct UpgradeTexts
{
    public string name;
    public TMP_Text text;
}
public class TowerStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TowerType towerType;
    public GameObject stats;
    public GameObject tower;
    public TMP_Text[] fightingTexts;
    public TMP_Text[] generatingTexts;
    public GameObject panel;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        stats.SetActive(true);
        switch (towerType)
        {
            case TowerType.fighting:
            {
                panel.SetActive(true);
                var combatTower = tower.GetComponent<CombatTower>();
                //set damage text
                fightingTexts[0].text = combatTower.baseDamage.ToString();
                //set range text
                fightingTexts[1].text = combatTower.detectionRange.ToString();
                //set speed text
                fightingTexts[2].text = combatTower.baseReloadSpeed.ToString();
                //set type to target text
                fightingTexts[3].text = combatTower.typeToTarget.ToString();
                //set name
                var towerName = combatTower.name.Replace("(Clone)", "");
                fightingTexts[4].text = towerName;
                //you're welcome nobe
                break;
            }
            case TowerType.generating:
            {
                panel.SetActive(true);
                var generatingTower = tower.GetComponent<GeneratingTower>();
                //set base point generation text
                generatingTexts[0].text = generatingTower.basePointsGenerated.ToString();
                //set speed text
                generatingTexts[1].text = generatingTower.timeBetweenPoints.ToString();
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        stats.SetActive(false);
        panel.SetActive(false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        stats.SetActive(false);
    }
}
