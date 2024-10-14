using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    public TowerUpgrading upgradePanel;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
            if (hit.collider.GetComponent<CombatTower>() != null)
            {
                if (hit.collider.GetComponent<CombatTower>().interactable)
                {
                    upgradePanel.UpdatePanel(hit.collider.GetComponent<Tower>());
                }
            }
        }
    }
}
