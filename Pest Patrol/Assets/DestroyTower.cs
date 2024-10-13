using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class DestroyTower : MonoBehaviour
{
    private Tower currentTower;
    public TowerUpgrading towerUpgrading;
    public UIFoldout foldout;
    
    public void Destroy()
    {
        currentTower = towerUpgrading.currentTowerGO;
        foldout.SetState(false);
        Destroy(currentTower.gameObject);
    }
}
