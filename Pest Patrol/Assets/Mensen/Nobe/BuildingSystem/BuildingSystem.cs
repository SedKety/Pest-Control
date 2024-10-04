using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum BuildState
{
    TileBuild,
    TowerBuild,
}

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    public TowerSO selectedTowerSO;
    public Transform currentBuildingTower;
    public LayerMask groundLayer;
    public GameObject pathPhaseGO, towerPhaseGO;
    public float placementHeightOffset = 0.5f;
    public float minDistance = 1f;
    public bool tileBuildingMode;
    public BuildState state;

    public List<GameObject> towersGO = new();
    private static List<Transform> wayPoints = new();

    private bool isPlacingTower;

    public Material validMaterial;
    public Material invalidMaterial;

    private List<Vector3> spotsToDrawGizmo;

    public Transform lastPlacedTileSnapPoint;
    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("EnterTowerPhase")]
    public void EnterTowerPhase()
    {
        state = BuildState.TowerBuild;
        pathPhaseGO.SetActive(false);
        towerPhaseGO.SetActive(true);

        GameManager.wayPoints.AddRange(wayPoints);
        DestroyCurrentTowerPreview();
    }

    public void SwitchTower(TowerSO newTowerSO)
    {
        if (GameManager.points >= newTowerSO.towerBuildPointCost)
        {
            if (newTowerSO == null)
            {
                Debug.LogError("Selected TowerSO is null.");
                return;
            }

            if (newTowerSO == selectedTowerSO)
            {
                DestroyCurrentTowerPreview();
                selectedTowerSO = null;
                return;
            }

            selectedTowerSO = newTowerSO;
            DestroyCurrentTowerPreview();

            CreateTowerGhostAtMousePosition();
        }
        else
        {
            print("Too poor");
        }
    }

    private void CreateTowerGhostAtMousePosition()
    {
        if (selectedTowerSO == null)
        {
            Debug.LogError("selectedTowerSO is null.");
            return;
        }

        if (selectedTowerSO.towerGhostGO == null)
        {
            Debug.LogError("Selected tower does not have a valid towerGhostGO.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            currentBuildingTower = Instantiate(selectedTowerSO.towerGhostGO, hit.point, Quaternion.identity).transform;
        }
        else
        {
            Debug.LogWarning("Raycast did not hit the ground layer.");
        }
    }

    private void DestroyCurrentTowerPreview()
    {
        if (currentBuildingTower != null)
        {
            Destroy(currentBuildingTower.gameObject);
            currentBuildingTower = null;
        }
    }

    public void PlaceTower(Vector3 position, TowerSO tower)
    {
        Vector3 posToAdd = new(0, 0, -3);
        Ray ray = new Ray(currentBuildingTower.GetComponent<Renderer>().bounds.center + new Vector3(0, 15, 0), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Debug.DrawLine(hit.point + new Vector3(0, 10, 0), hit.point, Color.black, 100);
            if (((1 << hit.collider.gameObject.layer) & groundLayer.value) != 0)
            {
                if (isPlacingTower || !IsValidTowerPosition() || !IsGroundBeneath(currentBuildingTower.transform.position)) return;
                if (GameManager.points < selectedTowerSO.towerBuildPointCost) return;
                isPlacingTower = true;
                GameManager.DeletePoints(selectedTowerSO.towerBuildPointCost);
                GameObject newTower = Instantiate(tower.towerToPlaceGO, position, currentBuildingTower.transform.rotation);
                if (state == BuildState.TowerBuild)
                {
                    towersGO.Add(newTower);
                    newTower.GetComponent<Tower>().MakeInteractable();
                }
                DestroyCurrentTowerPreview();
                selectedTowerSO = null;
                isPlacingTower = false;
            }
            else
            {
                Debug.LogWarning("Invalid placement: The object beneath the tower is not ground.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid placement: Raycast did not hit any object.");
        }
    }

    public void PlaceTile(TowerSO tower, Vector3 position)
    {
        if (GameManager.points < selectedTowerSO.towerBuildPointCost) return;
        isPlacingTower = true;
        GameManager.DeletePoints(selectedTowerSO.towerBuildPointCost);
        GameObject newTower = Instantiate(tower.towerToPlaceGO, position, currentBuildingTower.transform.rotation);
        DestroyCurrentTowerPreview();
        selectedTowerSO = null;
        isPlacingTower = false;
        lastPlacedTileSnapPoint = newTower.GetComponent<IPathTileInterface>().GetSnapPoint();
    }
    private bool IsValidTowerPosition()
    {
        Vector3 center = currentBuildingTower.GetComponent<BoxCollider>().bounds.center;
        Vector3 halfExtents = currentBuildingTower.GetComponent<BoxCollider>().bounds.extents;

        Collider[] colliders = Physics.OverlapBox(center, halfExtents);

        colliders = colliders.Where(c => c.transform != currentBuildingTower.transform &&
                                         !currentBuildingTower.transform.IsChildOf(c.transform)).ToArray();

        int blockingColliders = colliders.Count(c => ((1 << c.gameObject.layer) & groundLayer.value) == 0 &&
                                                     c.gameObject.transform != transform);

        if (blockingColliders > selectedTowerSO.allowedOverlaps)
        {
            return false;
        }

        return true;
    }


    public LayerMask LayerHit(RaycastHit hit)
    {
        return hit.collider.gameObject.layer;
    }

    public void DeleteTower(GameObject tower)
    {
        if (towersGO.Contains(tower))
        {
            Destroy(tower);
            towersGO.Remove(tower);
        }
    }

    private void Update()
    {
        if (currentBuildingTower == null || selectedTowerSO == null) return;

        if (state == BuildState.TowerBuild)
        {
            UpdateTowerPosition();  
        }
        else if (state == BuildState.TileBuild)
        {
            if (lastPlacedTileSnapPoint != null)
            {
                SnapToTile(lastPlacedTileSnapPoint);
                HandlePlacementInput(currentBuildingTower.position);
            }
        }
    }

    private void UpdateTowerPosition()
    {
        if (state != BuildState.TowerBuild) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector3 targetPosition = hit.point;

            currentBuildingTower.position = targetPosition;
            if (IsValidTowerPosition() && IsGroundBeneath(targetPosition))
            {
                SetTowerMaterial(validMaterial);
            }
            else
            {
                SetTowerMaterial(invalidMaterial);
            }

            HandlePlacementInput(targetPosition);
        }
    }
    private bool IsGroundBeneath(Vector3 position)
    {
        if (state == BuildState.TowerBuild)
        {
            List<Vector3> raycastPos = new List<Vector3> { position };

            foreach (var raycaster in currentBuildingTower.GetComponent<GhostTower>().raycasters)
            {
                raycastPos.Add(raycaster.position);
            }

            return GroundCheck(raycastPos);
        }
        else if (state == BuildState.TileBuild)
        {
            Ray ray = new Ray(position + Vector3.up * 5, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 20))
            {
                if (((1 << hit.collider.gameObject.layer) & groundLayer.value) == 0)
                {
                    return false;
                }
            }
        }
        return false;
    }



    public bool GroundCheck(List<Vector3> raycastPos)
    {
        for (int i = 0; i < raycastPos.Count; i++)
        {
            Ray ray = new Ray(raycastPos[i] + Vector3.up * 5, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (((1 << hit.collider.gameObject.layer) & groundLayer.value) == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }


    private void SetTowerMaterial(Material material)
    {
        if (currentBuildingTower != null)
        {
            Renderer[] renderers = currentBuildingTower.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material = material;
            }
        }
    }

    private void UpdatePathPosition()
    {
        if (state != BuildState.TileBuild)
        {
            return;
        }

        currentBuildingTower.GetComponent<BoxCollider>().enabled = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var pathTile = hit.collider.GetComponent<IPathTileInterface>().GetSnapPoint();
            if (pathTile != null)
            {
                SnapToTile(pathTile);
                SetTowerMaterial(validMaterial);
            }
            else
            {
                SetTowerMaterial(invalidMaterial);
            }
        }
        else
        {
            SetTowerMaterial(invalidMaterial);
        }
    }
    private void SnapToTile(Transform snapPoint)
    {
        if (state == BuildState.TileBuild)
        {
            currentBuildingTower.position = snapPoint.position;
            currentBuildingTower.rotation = snapPoint.rotation;


            lastPlacedTileSnapPoint = snapPoint;

            SetTowerMaterial(validMaterial);
        }
    }

    private void HandlePlacementInput(Vector3 targetPosition)
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (state == BuildState.TileBuild)
            {
                PlaceTile(selectedTowerSO, targetPosition);
            }
            else if (state == BuildState.TowerBuild)
            {
                PlaceTower(targetPosition, selectedTowerSO);
            }

        }
        else if (Input.GetButtonDown("RotateTower") && state == BuildState.TowerBuild)
        {
            RotateTowerPreview();
        }
    }

    private void RotateTowerPreview()
    {
        if (currentBuildingTower != null)
        {
            currentBuildingTower.Rotate(0, 90, 0);
        }
    }

    [ContextMenu("EnterBuildMode")]
    public void EnterBuildMode()
    {
        GameObject referenceObject = FindGameObjectWithLayer(groundLayer);
        placementHeightOffset = referenceObject.transform.position.y + 0.5f;
    }

    [ContextMenu("ExitBuildMode")]
    public void ExitBuildMode()
    {
        DestroyCurrentTowerPreview();
    }

    public async void ClearTowers(GameObject clearButton)
    {
        var buttonText = clearButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Clearing...";

        clearButton.GetComponent<Button>().interactable = false;
        foreach (var tower in towersGO)
        {
            Destroy(tower);
            await Task.Delay(1);
        }

        towersGO.Clear();
        buttonText.text = "Clear";
        clearButton.GetComponent<Button>().interactable = true;
    }

    private GameObject FindGameObjectWithLayer(int layer)
    {
        return FindObjectsOfType<GameObject>().FirstOrDefault(obj => obj.layer == layer);
    }
    public void OnDestroy()
    {
        Instance = null;
    }
}