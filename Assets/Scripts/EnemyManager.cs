using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
class WaveEnemies
{
    public int basicEnemyCount;
    public int fastEnemyCount;
}

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private WaveEnemies[] currentWave;
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
        
        if (!waveCompleted && AreAllEnemiesDie())
        {
            waveCompleted = true;
            waveTimer = timeBetweenWaves;
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

    private bool AreAllEnemiesDie()
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
