using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private UI ui;
    private BuildSlot selectedBuildSlot;

    public WaveManager waveManager;
    public GridBuilder currentGrid;

    [Header("Build Material")]
    [SerializeField] private Material attackRangeMat;
    [SerializeField] private Material buildPreviewMat;

    private bool isMouseOverUI;

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        MakeSlotNotAvailableIfNeeded(waveManager, currentGrid);
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                // Return true or false whether BuildSlot component was found on the hit object
                bool isBuildSlotClicked = hit.collider.GetComponent<BuildSlot>() == null;

                if (isBuildSlotClicked)
                    CancelBuildAction();
            }
        }
    }

    public void MakeSlotNotAvailableIfNeeded(WaveManager waveManager, GridBuilder currentGrid)
    {
        foreach (var wave in waveManager.GetLevelWaves())
        {
            if (wave.nextWaveGrid == null)
                continue;

            List<GameObject> grid = currentGrid.GetTileSetup();
            List<GameObject> nextWaveGrid = wave.nextWaveGrid.GetTileSetup();

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
