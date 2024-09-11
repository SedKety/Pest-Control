using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    public bool buildMode = false;
    public GameObject selectedTower;
    public Transform currentBuildingTower;
    public GameObject currentlySelectedTower;
    public int groundLayer;
    public List<GameObject> towers = new();
    public void Update()
    {
        if (buildMode & selectedTower & currentBuildingTower)
        {
            UpdateTowerPosition();
        }
    }
    public void UpdateTowerPosition()
    {
        selectedTower.GetComponent<BoxCollider>().enabled = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (selectedTower & Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            currentBuildingTower.position = new Vector3(Mathf.RoundToInt(hit.point.x), hit.point.y, Mathf.RoundToInt(hit.point.z));
        }
        Physics.Raycast(ray, out RaycastHit layer, Mathf.Infinity);
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceTower(currentBuildingTower.position, selectedTower);
        }
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            DeleteTower(currentBuildingTower.position, selectedTower);
        }
    }
    
    public void PlaceTower(Vector3 pos, GameObject tower)
    {
        Collider[] colliders = Physics.OverlapBox(pos, tower.GetComponent<BoxCollider>().size / 2.5f);
        if (colliders.Length <= 1)
        {
            GameObject g = Instantiate(tower, pos, Quaternion.identity);
            g.GetComponent<BoxCollider>().enabled = true;
            towers.Add(g);
            Destroy(currentBuildingTower.gameObject);
        }
    }
    public void DeleteTower(Vector3 pos, GameObject tower)
    {
        Collider[] colliders = Physics.OverlapBox(pos, tower.GetComponent<BoxCollider>().size / 2);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer != groundLayer)
            {
                Destroy(col.gameObject);
            }
        }
    }
    public void SwitchTower(GameObject tower)
    {
        selectedTower = tower;
        if (currentBuildingTower)
        {
            Destroy(currentBuildingTower.gameObject);
        }
        if (currentlySelectedTower == selectedTower)
        {
            currentlySelectedTower = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == groundLayer)
            {
               currentBuildingTower = Instantiate(tower, hit.point, Quaternion.identity).transform;
            }
        }
        currentlySelectedTower = selectedTower;
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

    public async void ClearTowers(GameObject clearButton)
    {
        string text = clearButton.GetComponentInChildren<TMP_Text>().text;
        clearButton.GetComponent<Button>().interactable = false;
        clearButton.GetComponentInChildren<TMP_Text>().text = "Busy";
        foreach (var tower in towers)
        {
            Destroy(tower);
            await Task.Delay(1);
        }
        clearButton.GetComponent<Button>().interactable = true;
        clearButton.GetComponentInChildren<TMP_Text>().text = text;
    }
}
