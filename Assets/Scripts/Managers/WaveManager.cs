using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public int basicEnemyCount;
    public int fastEnemyCount;
    public GridBuilder nextWaveGrid;
    public EnemyPortal[] nextWavePortals;
}

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private float waveTimer;
    [SerializeField] private GridBuilder currentGrid;
    [SerializeField] private WaveDetails[] levelWaves;
    [SerializeField] private int waveIndex;

    private bool isGameBegun;
    private bool isWaveTimerEnabled;
    private bool isMakingNextWave;
    private List<EnemyPortal> enemyPortals;
    private UIInGame uiInGame;

    void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
        uiInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (isGameBegun == false)
            return;

        HandleWaveTiming();
    }

    [ContextMenu("Activate Wave Manager")]
    public void ActivateWaveManager()
    {
        isGameBegun = true;
        EnableWaveTimer(true);
    }

    public void HandleWaveCompletion()
    {
        if (AreAllEnemiesDead() == false || isMakingNextWave)
            return;

        isMakingNextWave = true;
        CheckForNewLayout();
        EnableWaveTimer(true);
    }

    private void HandleWaveTiming()
    {
        if (isWaveTimerEnabled == false)
            return;
            
        waveTimer -= Time.deltaTime;
        uiInGame.UpdateWaveTimerText(waveTimer);

        if (waveTimer <= 0)
            StartNewWave();
    }

    public void StartNewWave()
    {
        waveIndex++;
        GiveEnemiesToPortals();
        EnableWaveTimer(false);
        isMakingNextWave = false;
    }

    private void EnableWaveTimer(bool isEnable)
    {
        if (isWaveTimerEnabled == isEnable)
            return;

        waveTimer = timeBetweenWaves;
        isWaveTimerEnabled = isEnable; // To keep track of toggle status
        uiInGame.ToggleWaveTimerUI(isEnable);
    }

    private void GiveEnemiesToPortals()
    {
        List<GameObject> enemyList = GetNewEnemies();

        if (enemyList == null)
        {
            Debug.Log("No more wave to setup");
            return;
        }

        int portalIndex = 0;

        for (int i = 0; i < enemyList.Count; i++)
        {
            GameObject enemyToAdd = enemyList[i];
            EnemyPortal portal = enemyPortals[portalIndex];

            portal.AddEnemy(enemyToAdd);

            portalIndex++;

            if (portalIndex >= enemyPortals.Count)
                portalIndex = 0;
        }
    }

    private void CheckForNewLayout()
    {
        if (waveIndex >= levelWaves.Length)
            return;

        WaveDetails nextWave = levelWaves[waveIndex];

        if (nextWave.nextWaveGrid != null)
        {
            UpdateLevelTiles(nextWave.nextWaveGrid);
            EnableNewPortals(nextWave.nextWavePortals);
        }

        currentGrid.UpdateNewNavMesh();
    }

    private void UpdateLevelTiles(GridBuilder nextGrid)
    {
        List<GameObject> grid = currentGrid.GetTileSetup();
        List<GameObject> newGrid = nextGrid.GetTileSetup();

        for (int i = 0; i < grid.Count; i++)
        {
            TileSlot currentTile = grid[i].GetComponent<TileSlot>();
            TileSlot newTile = newGrid[i].GetComponent<TileSlot>();

            bool shouldBeUpdated = currentTile.GetMesh() != newTile.GetMesh() ||
                currentTile.GetMaterial() != newTile.GetMaterial() ||
                currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
                currentTile.transform.rotation != newTile.transform.rotation;

            if (shouldBeUpdated)
            {
                currentTile.gameObject.SetActive(false);
                newTile.gameObject.SetActive(true);
                newTile.transform.parent = currentGrid.transform;

                grid[i] = newTile.gameObject;
                Destroy(currentTile.gameObject);
            }
        }
    }

    private void EnableNewPortals(EnemyPortal[] newPortals)
    {
        foreach (EnemyPortal portal in newPortals)
        {
            portal.AssignWaveManager(this);
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }

    private bool AreAllEnemiesDead()
    {
        foreach (EnemyPortal portal in enemyPortals)
        {
            if (portal.GetActiveEnemies().Count > 0)
                return false;
        }

        return true;
    }

    private List<GameObject> GetNewEnemies()
    {
        // Check if there are still more waves available
        if (waveIndex >= levelWaves.Length)
            return null;

        List<GameObject> enemyList = new();

        for (int i = 0; i < levelWaves[waveIndex].basicEnemyCount; i++)
            enemyList.Add(basicEnemyPrefab);

        for (int i = 0; i < levelWaves[waveIndex].fastEnemyCount; i++)
            enemyList.Add(fastEnemyPrefab);

        return enemyList;
    }

    public WaveDetails[] GetLevelWaves() => levelWaves;
}
