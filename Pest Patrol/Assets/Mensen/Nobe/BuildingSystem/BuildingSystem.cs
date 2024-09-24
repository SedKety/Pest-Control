using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    }

    public void SwitchTower(TowerSO newTowerSO)
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
        Ray ray = new Ray(position + Vector3.up * 5, Vector3.down); 
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (((1 << hit.collider.gameObject.layer) & groundLayer.value) != 0)
            {
                if (isPlacingTower || !IsValidTowerPosition(position)) return;

                isPlacingTower = true;

                GameObject newTower = Instantiate(tower.towerToPlaceGO, position, currentBuildingTower.rotation);
                newTower.GetComponentInChildren<BoxCollider>().enabled = true;
                if(state == BuildState.TowerBuild)
                {
                    towersGO.Add(newTower);
                }
                DestroyCurrentTowerPreview();

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

    public LayerMask LayerHit(RaycastHit hit)
    {
        return hit.collider.gameObject.layer;  
    }

    public void DeleteTower(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, currentBuildingTower.GetComponent<BoxCollider>().size * 0.75f);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer != groundLayer)
            {
                Destroy(col.gameObject);
                towersGO.Remove(col.gameObject);
            }
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
            UpdatePathPosition(); 
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

            if (IsValidTowerPosition(targetPosition) && IsGroundBeneath(targetPosition))
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
        Ray ray = new Ray(position + Vector3.up * 5, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return ((1 << hit.collider.gameObject.layer) & groundLayer.value) != 0;
        }
        return false;
    }

    private bool IsValidTowerPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, currentBuildingTower.GetComponent<BoxCollider>().size * 0.75f);
        int blockingColliders = colliders.Count(c => c.gameObject.layer != groundLayer);

        if (blockingColliders > selectedTowerSO.allowedOverlaps)
        {
            return false;
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

        currentBuildingTower.GetComponentInChildren<BoxCollider>().enabled = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var pathTile = hit.collider.GetComponent<IPathTileInterface>();
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

    private void SnapToTile(IPathTileInterface tile)
    {
        if (state == BuildState.TileBuild) 
        {
            Transform snapPoint = tile.GetSnapPoint();
            currentBuildingTower.position = snapPoint.position;
            currentBuildingTower.rotation = snapPoint.rotation;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceTower(currentBuildingTower.position, selectedTowerSO);
            }
        }
    }

    private void HandlePlacementInput(Vector3 targetPosition)
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceTower(targetPosition, selectedTowerSO);
        }
        else if (Input.GetButtonDown("RotateTower"))
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
}