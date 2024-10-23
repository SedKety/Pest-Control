using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    public TowerUpgrading upgradePanel;
    public EnemyStats enemyPanel;
    public LayerMask triggerLayer;
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, triggerLayer)) return;
        if (hit.collider.GetComponent<CombatTower>() != null)
        {
            if (hit.collider.GetComponent<CombatTower>().interactable)
            {
                upgradePanel.UpdatePanel(hit.collider.GetComponent<Tower>());
            }
        }
        if (hit.collider.GetComponent<Enemy>() != null)
        {
            enemyPanel.UpdatePanel(hit.collider.GetComponent<Enemy>());
        }
    }
}
