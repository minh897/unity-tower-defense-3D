using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public WaveManager waveManager;
    public GridBuilder currentGrid;

    [SerializeField] private LayerMask whatToIgnore;

    [Header("Build Material")]
    [SerializeField] private Material attackRangeMat;
    [SerializeField] private Material buildPreviewMat;

    [Header("Build Details")]
    [SerializeField] private float towerCenterY = .5f;
    [SerializeField] private float cameraShakeDuration = .15f;
    [SerializeField] private float cameraShakeMagnitude = .02f;

    private bool isMouseOverUI;
    
    private UI ui;
    private BuildSlot selectedBuildSlot;
    private GameManager gameManager;
    private CameraEffects cameraEffects;

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();

        MakeSlotNotAvailableIfNeeded(waveManager, currentGrid);
    }

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildAction();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isMouseOverUI)
                return;

            // Collect hit collider info by using Raycast from the camera to the clicked mouse position
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, ~whatToIgnore))
            {
                // Return true or false whether BuildSlot component was found on the hit object
                bool isBuildSlotNotClicked = hit.collider.GetComponent<BuildSlot>() == null;

                if (isBuildSlotNotClicked)
                    CancelBuildAction();
            }
        }
    }

    public void UpdateBuildManager(WaveManager newWaveManager)
    {
        MakeSlotNotAvailableIfNeeded(newWaveManager, currentGrid);
    }

    public void BuildTower(GameObject towerToBuild, int towerPrice, Transform newTowerPreview)
    {
        if (gameManager.HasEnoughCurrency(towerPrice) == false)
        {
            ui.uiInGame.ShakeCurrencyUI();
            return;
        }

        if (towerToBuild == null)
        {
            Debug.LogWarning("Didn't tower assigned to this button");
            return;
        }

        // Check if we have the current selected button
        if (ui.uiBuildButton.GetLastSelectedButton() == null)
            return;

        Transform towerPreview = newTowerPreview;
        BuildSlot slotToUse = GetSelectedBuildSlot();
        CancelBuildAction();

        slotToUse.SnapToDefaultPosition();
        slotToUse.SetSlotAvailable(false);

        ui.uiBuildButton.SetLastSelected(null, null);

        cameraEffects.ShakeScreen(cameraShakeDuration, cameraShakeMagnitude);

        GameObject newTower = Instantiate(towerToBuild, slotToUse.GetBuildPosition(towerCenterY), Quaternion.identity);
        newTower.transform.rotation = towerPreview.rotation;
    }

    public void MakeSlotNotAvailableIfNeeded(WaveManager waveManager, GridBuilder currentGrid)
    {
        foreach (var wave in waveManager.GetLevelWaves())
        {
            if (wave.waveGrid == null)
                continue;

            List<GameObject> grid = currentGrid.GetTileSetup();
            List<GameObject> nextWaveGrid = wave.waveGrid.GetTileSetup();

            for (int i = 0; i < grid.Count; i++)
            {
                TileSlot currentTile = grid[i].GetComponent<TileSlot>();
                TileSlot nextTile = nextWaveGrid[i].GetComponent<TileSlot>();

                bool isTileNotTheSame = currentTile.GetMesh() != nextTile.GetMesh() ||
                    currentTile.GetMaterial() != nextTile.GetMaterial() ||
                    currentTile.GetAllChildren().Count != nextTile.GetAllChildren().Count;

                if (isTileNotTheSame == false)
                    continue;

                if (grid[i].TryGetComponent<BuildSlot>(out var buildSlot))
                    buildSlot.SetSlotAvailable(false);
            }
        }
    }

    public void CancelBuildAction()
    {
        if (selectedBuildSlot == null)
            return;
            
        ui.uiBuildButton.GetLastSelectedButton()?.TogglePreviewVisual(false);

        selectedBuildSlot.UnSelectTile();
        selectedBuildSlot = null;
        DisableBuildMenu();
    }

    public void SelectBuildSlot(BuildSlot buildSlot)
    {
        if (selectedBuildSlot != null)
            selectedBuildSlot.UnSelectTile();

        selectedBuildSlot = buildSlot;
    }

    public void EnableBuildMenu()
    {
        if (selectedBuildSlot != null)
            return;

        ui.uiBuildButton.ShowBuildButtons(true);
    }

    private void DisableBuildMenu()
    {
        ui.uiBuildButton.ShowBuildButtons(false);
    }

    public void OnMouseOverUI(bool isOverUI) => isMouseOverUI = isOverUI;

    public BuildSlot GetSelectedBuildSlot() => selectedBuildSlot;

    public Material GetAttackRangeMaterial() => attackRangeMat;

    public Material GetBuildPreviewMaterial() => buildPreviewMat;
}
