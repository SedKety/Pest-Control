using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    public bool buildMode = false;
    public TowerSO selectedTowerSO;
    public Transform currentBuildingTower;
    public TowerSO currentlySelectedTowerSO;
    public int groundLayer;
    public List<GameObject> towersGO = new();
    public float y;
    public void Update()
    {
        if (buildMode && selectedTowerSO && currentBuildingTower)
        {
            UpdateTowerPosition();
        }
    }
    public void UpdateTowerPosition()
    {
        currentBuildingTower.GetComponentInChildren<BoxCollider>().enabled = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (selectedTowerSO & Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            currentBuildingTower.position = new Vector3(Mathf.RoundToInt(hit.point.x), y, Mathf.RoundToInt(hit.point.z));
        }
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceTower(currentBuildingTower.position, selectedTowerSO);
        }
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            DeleteTower(currentBuildingTower.position, selectedTowerSO);
        }
        if (Input.GetButtonDown("RotateTower") && !EventSystem.current.IsPointerOverGameObject())
        {
            currentBuildingTower.Rotate(0, 90, 0);
            Debug.Log("manhandle me daddy :3");
        }
    }
    public void PlaceTower(Vector3 pos, TowerSO tower)
    {
        List<Collider> colliders = Physics.OverlapBox(pos, currentBuildingTower.GetComponentInChildren<BoxCollider>().size / 2.5f).ToList();
        if (colliders.Count <= 1)
        {
            GameObject g = Instantiate(tower.towerToPlaceGO, pos, currentBuildingTower.rotation);
            g.GetComponentInChildren<BoxCollider>().enabled = true;
            towersGO.Add(g);
            Destroy(currentBuildingTower.gameObject);
        }
        else
        {
            Debug.Log(colliders.Count);
        }
    }
    public void DeleteTower(Vector3 pos, TowerSO tower)
    {
        Collider[] colliders = Physics.OverlapBox(pos, currentBuildingTower.GetComponentInChildren<BoxCollider>().size / 2.5f);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer != groundLayer)
            {
                Destroy(col.gameObject);
            }
        }
    }
    public void SwitchTower(TowerSO tower)
    {
        selectedTowerSO = tower;
        if (currentBuildingTower)
        {
            Destroy(currentBuildingTower.gameObject);
        }
        if (currentlySelectedTowerSO == selectedTowerSO)
        {
            currentBuildingTower = null;
            currentlySelectedTowerSO = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && hit.collider.gameObject.layer == groundLayer)
        {
            currentBuildingTower = Instantiate(tower.towerGhostGO.transform, hit.point, Quaternion.identity);
        }
        currentlySelectedTowerSO = selectedTowerSO;
    }
    [ContextMenu("EnterBuildMode")]
    public void EnterBuildMode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
        y = hit.point.y;
        buildMode = true;
    }
    [ContextMenu("ExitBuildMode")]
    public void ExitBuildMode()
    {
        buildMode = false;
        Destroy(currentBuildingTower.gameObject);
    }
    public async void ClearTowers(GameObject clearButton)
    {
        string text = clearButton.GetComponentInChildren<TMP_Text>().text;
        clearButton.GetComponent<Button>().interactable = false;
        clearButton.GetComponentInChildren<TMP_Text>().text = "Busy";
        foreach (var tower in towersGO)
        {
            Destroy(tower);
            await Task.Delay(1);
        }
        clearButton.GetComponent<Button>().interactable = true;
        clearButton.GetComponentInChildren<TMP_Text>().text = text;
    }
}
