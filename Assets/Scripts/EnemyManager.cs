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
    }


    [ContextMenu("Setup Next Wave")]
    private void SetupNextWave()
    {
        List<GameObject> newEnemies = CreateEnemywave();

        int portalIndex = 0;
        for (int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portal = enemyPortals[portalIndex];
            portal.GetEnemyList().Add(enemyToAdd);
            portalIndex++;

            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
    }


    private List<GameObject> CreateEnemywave()
    {
        List<GameObject> newEnemyWave = new List<GameObject>();

        for (int i = 0; i < currentWave.basicEnemyCount; i++)
        {
            newEnemyWave.Add(basicEnemyPrefab);
        }

        for (int i = 0; i < currentWave.fastEnemyCount; i++)
        {
            newEnemyWave.Add(fastEnemyPrefab);
        }

        return newEnemyWave;
    }

}
