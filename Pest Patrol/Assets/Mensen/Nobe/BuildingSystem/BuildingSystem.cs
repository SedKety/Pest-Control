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
    public static BuildingSystem Instance;
    public TowerSO selectedTowerSO;
    public Transform currentBuildingTower;
    public TowerSO currentlySelectedTowerSO;
    public int groundLayer;
    public List<GameObject> towersGO = new();
    public float y;
    public bool tileBuildingMode;
    public GameObject pathPhaseGO, towerPhaseGO;
    public float minDistance;


    private static List<Transform> wayPoints = new();

    public void Awake()
    {
        Instance = this;
    }
    public void Update()
    {
        if (selectedTowerSO && currentBuildingTower)
        {
            UpdateTowerPosition();
        }
    }
    [ContextMenu("EnterTowerPhase")]
    public void EnterTowerPhase()
    {
        pathPhaseGO.SetActive(false);
        towerPhaseGO.SetActive(true);
        for (int i = 0; i < wayPoints.Count; i++)
        {
            GameManager.wayPoints.Add(wayPoints[i]);
        }
    }
    public void UpdateTowerPosition()
    {
        currentBuildingTower.GetComponentInChildren<BoxCollider>().enabled = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (selectedTowerSO & Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (tileBuildingMode)
            {
                currentBuildingTower.position = new Vector3(Mathf.RoundToInt(hit.point.x), y, Mathf.RoundToInt(hit.point.z));
                if (towersGO.Count != 0)
                {
                    List<PathTile> list = FindObjectsByType<PathTile>(FindObjectsSortMode.None).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int x = 0; x < list[i].snapPoints.Count; x++)
                        {
                            if (Vector3.Distance(list[i].snapPoints[x].position, currentBuildingTower.position) < minDistance)
                            {
                                currentBuildingTower.position = list[i].snapPoints[x].position;
                                minDistance = Vector3.Distance(list[i].snapPoints[x].position, currentBuildingTower.position);
                            }
                        }
                    }
                }
                minDistance = 5;
            }
            else
            {
                currentBuildingTower.position = new Vector3(Mathf.RoundToInt(hit.point.x), y, Mathf.RoundToInt(hit.point.z));
            }
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

    public void CheckForPath(GameObject placedGO)
    {
        PathTile pathTile = placedGO.GetComponent<PathTile>();
        if (pathTile != null && pathTile.wayPoints != null && pathTile.wayPoints.Length > 0)
        {
            Transform[] waypointsToAdd = pathTile.shouldFlipWayPoints ? pathTile.wayPoints.Reverse().ToArray() : pathTile.wayPoints;

            for (int i = 0; i < waypointsToAdd.Length; i++)
            {
                wayPoints.Add(waypointsToAdd[i]);
                print(wayPoints.Count);
            }
        }
    }
    public void PlaceTower(Vector3 pos, TowerSO tower)
    {
        List<Collider> colliders = Physics.OverlapBox(pos, currentBuildingTower.GetComponentInChildren<BoxCollider>().size / 2.5f).ToList();
        if (colliders.Count <= 1)
        {
            GameObject GO = Instantiate(tower.towerToPlaceGO, pos, currentBuildingTower.rotation);
            GO.GetComponentInChildren<BoxCollider>().enabled = true;
            towersGO.Add(GO);
            CheckForPath(GO);
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
    static public GameObject FindGameObjectWithLayer(int layer)
    {
        var arrayGO = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        GameObject go = null;
        for (int i = 0; i < arrayGO.Length; i++)
        {
            if (arrayGO[i].layer == layer) { go = arrayGO[i]; }
        }
        return go;
    }
    [ContextMenu("EnterBuildMode")]
    public void EnterBuildMode()
    {
        GameObject go = FindGameObjectWithLayer(3);
        y = go.transform.position.y;
    }
    [ContextMenu("ExitBuildMode")]
    public void ExitBuildMode()
    {
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
    public void SwitchModes()
    {
        tileBuildingMode = false;
        if (currentBuildingTower)
        {
            Destroy(currentBuildingTower.gameObject);
            currentBuildingTower = null;
            currentlySelectedTowerSO = null;
        }
    }
}
