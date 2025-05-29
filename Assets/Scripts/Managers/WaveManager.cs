using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class WaveDetails
{
    public int basicEnemyCount;
    public int fastEnemyCount;
    public GridBuilder newGrid;
    public EnemyPortal[] newPortals;
}

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private GridBuilder currentGrid;
    [SerializeField] private WaveDetails[] currentWave;

    private bool waveCompleted;
    private int waveIndex;
    private float waveTimer;

    private float checkInterval = .5f;
    private float nextCheckTime;
    private List<EnemyPortal> enemyPortals;

    void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
    }

    void Start()
    {
        SetupNextWave();
    }

    void Update()
    {
        HandleWaveCompletion();
        HandleWaveTiming();
    }

    private void HandleWaveTiming()
    {
        if (waveCompleted)
        {
            waveTimer -= Time.deltaTime;

            if (waveTimer <= 0)
            {
                SetupNextWave();
            }
        }
    }

    private void HandleWaveCompletion()
    {
        if (IsCheckReady() == false)
        {
            return;
        }

        if (!waveCompleted && AreAllEnemiesDead())
        {
            waveCompleted = true;
            waveTimer = timeBetweenWaves;
            CheckForNewLayout();
        }
    }

    [ContextMenu("Setup Next Wave")]
    private void SetupNextWave()
    {
        waveCompleted = false;

        List<GameObject> enemyList = CreateEnemyWave();

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
            {
                portalIndex = 0;
            }
        }
    }

    private List<GameObject> CreateEnemyWave()
    {
        // Check if there are still more waves available
        if (waveIndex >= currentWave.Length)
        {
            return null;
        }

        List<GameObject> enemyList = new();

        for (int i = 0; i < currentWave[waveIndex].basicEnemyCount; i++)
        {
            enemyList.Add(basicEnemyPrefab);
        }

        for (int i = 0; i < currentWave[waveIndex].fastEnemyCount; i++)
        {
            enemyList.Add(fastEnemyPrefab);
        }

        waveIndex++;

        return enemyList;
    }

    private void CheckForNewLayout()
    {
        if (waveIndex >= currentWave.Length)
        {
            return;
        }

        WaveDetails nextWave = currentWave[waveIndex];

        if (nextWave.newGrid != null)
        {
            UpdateLevelTiles(nextWave.newGrid);
            EnableNewPortals(nextWave.newPortals);
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
                currentTile.GetAllChildren() != newTile.GetAllChildren() ||
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
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }

    private bool AreAllEnemiesDead()
    {
        foreach (EnemyPortal portal in enemyPortals)
        {
            if (portal.GetActiveEnemies().Count > 0)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsCheckReady()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            return true;
        }

        return false;
    }
}
