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

    [Header("Respawn Point")]
    [SerializeField] private Transform respawn;
    
    [Header("Wave Details")]
    [SerializeField] private WaveEnemies currentWave;
    [SerializeField] private float spawnCoolDown;
    
    private float spawnTimer;
    private List<GameObject> enemiesToCreate;

    void Start()
    {
        enemiesToCreate = CreateEnemywave();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            CreateEnemy();
            spawnTimer = spawnCoolDown;
        }
    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, respawn.position, Quaternion.identity);
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

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject choosenEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.Remove(choosenEnemy);
        return choosenEnemy;
    }
}
