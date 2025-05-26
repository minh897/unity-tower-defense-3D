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
    [SerializeField] private WaveEnemies currentWave;

    public List<EnemyPortal> enemyPortals;


    void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));

        SetupNextWave();
    }

    [ContextMenu("Setup Next Wave")]
    private void SetupNextWave()
    {
        List<GameObject> enemyList = CreateEnemyWave();

        int portalIndex = 0;
        for (int i = 0; i < enemyList.Count; i++)
        {
            GameObject enemyToAdd = enemyList[i];
            EnemyPortal portal = enemyPortals[portalIndex];

            portal.GetEnemyList().Add(enemyToAdd);

            portalIndex++;

            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
    }


    private List<GameObject> CreateEnemyWave()
    {
        List<GameObject> enemyList = new();

        for (int i = 0; i < currentWave.basicEnemyCount; i++)
        {
            enemyList.Add(basicEnemyPrefab);
        }

        for (int i = 0; i < currentWave.fastEnemyCount; i++)
        {
            enemyList.Add(fastEnemyPrefab);
        }

        return enemyList;
    }

}
