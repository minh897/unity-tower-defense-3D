using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public int basicEnemyCount;
    public int fastEnemyCount;
    public int swarmEnemyCount;
    public int heavyEnemyCount;
    public int stealthEnemyCount;
    public int flyingEnemyCount;
    public int flyingBossEnemyCount;
    public int spiderBossEnemyCount;
    public GridBuilder waveGrid;
    // public EnemyPortal[] wavePortals;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GridBuilder currentGrid;
    [SerializeField] private NavMeshSurface flyingNavSurface;
    [SerializeField] private NavMeshSurface droneNavSurface;
    [SerializeField] private MeshCollider[] flyingNavColliders;
    [Space]

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private GameObject swarmEnemyPrefab;
    [SerializeField] private GameObject heavyEnemyPrefab;
    [SerializeField] private GameObject stealthEnemyPrefab;
    [SerializeField] private GameObject flyingEnemyPrefab;
    [SerializeField] private GameObject flyingBossEnemyPrefab;
    [SerializeField] private GameObject spiderBossEnemyPrefab;

    // [Header("Level Update Details")]
    // [SerializeField] private float yOffset = 5;
    // [SerializeField] private float tileDelay = .1f;

    [Header("Wave Settings")]
    [SerializeField] private int waveIndex;
    [SerializeField] private WaveDetails[] levelWaves;

    private bool isGameBegun;
    private bool isMakingNextWave;
    private bool isNextWaveButtonEnabled;
    private List<EnemyPortal> enemyPortals;
    private List<GameObject> enemyList;
    private UIInGame uiInGame;
    // private TileAnimator tileAnimator;
    private GameManager gameManager;

    void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));

        uiInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
        gameManager = FindFirstObjectByType<GameManager>();
        // tileAnimator = FindFirstObjectByType<TileAnimator>();

        flyingNavColliders = GetComponentsInChildren<MeshCollider>();
    }

    void Update()
    {
        if (isGameBegun == false)
            return;

        if (Input.GetKeyDown(KeyCode.T))
            ActivateWaveManager();
    }

    public void ActivateWaveManager()
    {
        isGameBegun = true;
        uiInGame = gameManager.uiInGame;
        EnableNextWaveUI(true);
    }

    private void UpdateNavMeshes()
    {
        foreach (var collider in flyingNavColliders)
        {
            collider.enabled = true;
        }

        flyingNavSurface.BuildNavMesh();

        foreach (var collider in flyingNavColliders)
        {
            collider.enabled = false;
        }

        currentGrid.UpdateNewNavMesh();
        droneNavSurface.BuildNavMesh();
    }

    public void UpdateDroneNavMesh()
    {
        droneNavSurface.BuildNavMesh();
    }

    public void HandleWaveCompletion()
    {
        // Stop next wave when WaveManager is disabled
        if (isGameBegun == false)
            return;

        if (isMakingNextWave == true)
            return;

        isMakingNextWave = true;
        waveIndex++;

        if (HasNoMoreWave())
        {
            gameManager.CompleteLevel();
            return;
        }

        EnableNextWaveUI(true);

        // if (HasNewLayout())
        //     AttemptToUpdateLayout();
        // else
        //     EnableNextWaveUI(true);
    }

    public void StartNewWave()
    {
        UpdateNavMeshes();
        GiveEnemiesToPortals();
        EnableNextWaveUI(false);
        isMakingNextWave = false;
    }

    public void EnableNextWaveUI(bool isEnable)
    {
        if (isNextWaveButtonEnabled == isEnable)
            return;

        isNextWaveButtonEnabled = isEnable; // To keep track of toggle status
        uiInGame.ToggleNextWaveButton(isEnable);
    }

    private void GiveEnemiesToPortals()
    {
        enemyList = GetNewEnemies();

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

        uiInGame.UpdateEnemyCountText(enemyList.Count);
    }

    // private bool AreAllEnemiesDead()
    // {
    //     foreach (EnemyPortal portal in enemyPortals)
    //     {
    //         if (portal.GetActiveEnemies().Count > 0)
    //             return false;
    //     }

    //     return true;
    // }

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

        for (int i = 0; i < levelWaves[waveIndex].swarmEnemyCount; i++)
            enemyList.Add(swarmEnemyPrefab);

        for (int i = 0; i < levelWaves[waveIndex].heavyEnemyCount; i++)
            enemyList.Add(heavyEnemyPrefab);

        for (int i = 0; i < levelWaves[waveIndex].stealthEnemyCount; i++)
            enemyList.Add(stealthEnemyPrefab);

        for (int i = 0; i < levelWaves[waveIndex].flyingEnemyCount; i++)
            enemyList.Add(flyingEnemyPrefab);

        for (int i = 0; i < levelWaves[waveIndex].flyingBossEnemyCount; i++)
            enemyList.Add(flyingBossEnemyPrefab);

        for (int i = 0; i < levelWaves[waveIndex].spiderBossEnemyCount; i++)
            enemyList.Add(spiderBossEnemyPrefab);

        return enemyList;
    }

    public WaveDetails[] GetLevelWaves() => levelWaves;

    public int GetEnemyCount() => enemyList.Count;

    private bool HasNoMoreWave() => waveIndex >= levelWaves.Length;

    public void DeactivateWaveManager() => isGameBegun = false;

    /* USE FOR UPDATING NEW GRID LAYOUT IN THE SAME LEVEL */

    // private void UpdateLevelTiles(WaveDetails nextWave)
    // {
    //     GridBuilder nextGrid = nextWave.waveGrid;
    //     List<GameObject> grid = currentGrid.GetTileSetup();
    //     List<GameObject> newGrid = nextGrid.GetTileSetup();

    //     if (grid.Count != newGrid.Count)
    //     {
    //         Debug.LogWarning("current grid and new grid has different size");
    //         return;
    //     }

    //     List<TileSlot> tilesToRemove = new();
    //     List<TileSlot> tilesToAdd = new();

    //     for (int i = 0; i < grid.Count; i++)
    //     {
    //         TileSlot currentTile = grid[i].GetComponent<TileSlot>();
    //         TileSlot newTile = newGrid[i].GetComponent<TileSlot>();

    //         bool shouldBeUpdated = currentTile.GetMesh() != newTile.GetMesh() ||
    //             currentTile.GetMaterial() != newTile.GetMaterial() ||
    //             currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
    //             currentTile.transform.rotation != newTile.transform.rotation;

    //         if (shouldBeUpdated)
    //         {
    //             tilesToRemove.Add(currentTile);
    //             tilesToAdd.Add(newTile);
    //             grid[i] = newTile.gameObject;
    //         }
    //     }

    //     StartCoroutine(RebuildLevelCoroutine(tilesToRemove, tilesToAdd, nextWave, tileDelay));
    // }

    // private void AddTile(TileSlot newTile)
    // {
    //     newTile.gameObject.SetActive(true);
    //     newTile.transform.position += new Vector3(0, -5, 0);
    //     newTile.transform.parent = currentGrid.transform;

    //     Vector3 targetPosition = newTile.transform.position + new Vector3(0, yOffset, 0);
    //     tileAnimator.MoveTile(newTile.transform, targetPosition);
    // }

    // private void RemoveTile(TileSlot tileToRemove)
    // {
    //     Vector3 targetPosition = tileToRemove.transform.position + new Vector3(0, -yOffset, 0);
    //     tileAnimator.MoveTile(tileToRemove.transform, targetPosition);
    //     Destroy(tileToRemove.gameObject, 1);
    // }

    // private IEnumerator RebuildLevelCoroutine(List<TileSlot> tilesToRemove, List<TileSlot> tilesToAdd, WaveDetails waveDetails, float delay)
    // {
    //     for (int i = 0; i < tilesToRemove.Count; i++)
    //     {
    //         yield return new WaitForSeconds(delay);
    //         RemoveTile(tilesToRemove[i]);
    //     }

    //     for (int i = 0; i < tilesToAdd.Count; i++)
    //     {
    //         yield return new WaitForSeconds(delay);
    //         AddTile(tilesToAdd[i]);
    //     }

    //     EnableNewPortals(waveDetails.wavePortals);
    //     EnableNextWaveUI(true);
    // }

    // private void EnableNewPortals(EnemyPortal[] newPortals)
    // {
    //     foreach (EnemyPortal portal in newPortals)
    //     {
    //         portal.AssignWaveManager(this);
    //         portal.gameObject.SetActive(true);
    //         enemyPortals.Add(portal);
    //     }
    // }

    // private bool HasNewLayout() => waveIndex < levelWaves.Length && levelWaves[waveIndex].waveGrid != null;

    // private void AttemptToUpdateLayout() => UpdateLevelTiles(levelWaves[waveIndex]);

}
