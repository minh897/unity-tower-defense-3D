using System.Collections.Generic;
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

    
    [Header("Wave Details")]
    [SerializeField] private WaveEnemies[] currentWave;
    private int waveIndex;

    public List<EnemyPortal> enemyPortals;


    void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
    }

    void Start()
    {
        SetupNextWave();
    }

    [ContextMenu("Setup Next Wave")]
    private void SetupNextWave()
    {
        List<GameObject> enemyList = CreateEnemyWave();

        int portalIndex = 0;
        
        if (enemyList == null)
        {
            Debug.Log("No more wave to setup");
        }

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

}
