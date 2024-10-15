using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public Color normalColor;
    public Color selectedColor;
    public Image outline;
    private Vector3 originalScale;
    public float scaleFactor;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale *= scaleFactor;
        outline.GetComponent<Image>().color = selectedColor;
        stats.SetActive(true);
        switch (towerType)
        {
            case TowerType.fighting:
            {
                panel.SetActive(true);
                var combatTower = tower.GetComponent<CombatTower>();
                //set damage text
                fightingTexts[0].text = combatTower.baseDamages.ToString();
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
                var towerName = generatingTower.name.Replace("(Clone)", "");
                generatingTexts[2].text = towerName;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        outline.color = normalColor;
        stats.SetActive(false);
        panel.SetActive(false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        outline.color = normalColor;
        stats.SetActive(false);
    }
}
