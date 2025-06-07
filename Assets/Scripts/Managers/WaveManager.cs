using System.Collections;
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
    [SerializeField] private int waveIndex = -1;

    [Header("Level Update Details")]
    [SerializeField] private float yOffset = 5;
    [SerializeField] private float tileDelay = .1f;

    private bool isGameBegun;
    private bool isWaveTimerEnabled;
    private bool isMakingNextWave;
    private List<EnemyPortal> enemyPortals;
    private UIInGame uiInGame;
    private TileAnimator tileAnimator;

    void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
        uiInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
        tileAnimator = FindFirstObjectByType<TileAnimator>();
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

    private void AttemptToUpdateLayout() => UpdateLevelTiles(levelWaves[waveIndex]);

    private void UpdateLevelTiles(WaveDetails nextWave)
    {
        GridBuilder nextGrid = nextWave.nextWaveGrid;
        List<GameObject> grid = currentGrid.GetTileSetup();
        List<GameObject> newGrid = nextGrid.GetTileSetup();

        if (grid.Count != newGrid.Count)
        {
            Debug.LogWarning("current grid and new grid has different size");
            return;
        }

        List<TileSlot> tilesToRemove = new();
        List<TileSlot> tilesToAdd = new();

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
                tilesToRemove.Add(currentTile);
                tilesToAdd.Add(newTile);
                grid[i] = newTile.gameObject;
            }
        }

        StartCoroutine(RebuildLevelCoroutine(tilesToRemove, tilesToAdd, nextWave, tileDelay));
    }

    private IEnumerator RebuildLevelCoroutine(List<TileSlot> tilesToRemove, List<TileSlot> tilesToAdd, WaveDetails waveDetails, float delay)
    {
        for (int i = 0; i < tilesToRemove.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            RemoveTile(tilesToRemove[i]);
        }

        for (int i = 0; i < tilesToAdd.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            AddTile(tilesToAdd[i]);
        }

        EnableNewPortals(waveDetails.nextWavePortals);
        EnableWaveTimer(true);
    }

    private void AddTile(TileSlot newTile)
    {
        newTile.gameObject.SetActive(true);
        newTile.transform.position += new Vector3(0, -5, 0);
        newTile.transform.parent = currentGrid.transform;

        Vector3 targetPosition = newTile.transform.position + new Vector3(0, yOffset, 0);
        tileAnimator.MoveTile(newTile.transform, targetPosition);
    }

    private void RemoveTile(TileSlot tileToRemove)
    {
        Vector3 targetPosition = tileToRemove.transform.position + new Vector3(0, -yOffset, 0);
        tileAnimator.MoveTile(tileToRemove.transform, targetPosition);
        Destroy(tileToRemove.gameObject, 1);
    }

    private bool HasNewLayout() => waveIndex < levelWaves.Length && levelWaves[waveIndex].nextWaveGrid != null;

    private bool HasNoMoreWave() => waveIndex >= levelWaves.Length;

    public void HandleWaveCompletion()
    {
        if (AreAllEnemiesDead() == false || isMakingNextWave)
            return;

        isMakingNextWave = true;
        waveIndex++;

        if (HasNoMoreWave())
        {
            Debug.LogWarning("Level is completed");
            return;
        }

        if (HasNewLayout())
                AttemptToUpdateLayout();
            else
                EnableWaveTimer(true);

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
        currentGrid.UpdateNewNavMesh();
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
