using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public bool buildMode = false;
    public GameObject selectedTower;
    public Transform currentBuildingTower;
    public int groundLayer;
    public void Update()
    {
        if (buildMode & selectedTower & currentBuildingTower)
        {
            UpdateTowerPosition();
        }
    }
    public void UpdateTowerPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (selectedTower & Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            currentBuildingTower.position = hit.point;
        }
    }
    public void SwitchTower(GameObject tower)
    {
        selectedTower = tower;
        if (currentBuildingTower)
        {
            Destroy(currentBuildingTower.gameObject);
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == groundLayer)
            {
               currentBuildingTower = Instantiate(tower, hit.point, Quaternion.identity).transform;
            }
        }
    }
    [ContextMenu("EnterBuildMode")]
    public void EnterBuildMode()
    {
        buildMode = true;
    }

    public void ExitBuildMode()
    {
        buildMode = false;
        Destroy(currentBuildingTower.gameObject);
    }
}
