using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [Header("Respawn Details")]
    [SerializeField] private float spawnCoolDown;

    public List<Waypoint> waypoints;

    private float spawnTimer;
    private List<GameObject> enemiesToCreate;
    private List<GameObject> activeEnemies;

    void Awake()
    {
        enemiesToCreate = new();
        activeEnemies = new();

        CollectWaypoints();
    }

    void Update()
    {
        if (CanMakeNewEnemy())
        {
            CreateEnemy();
        }
    }

    private bool CanMakeNewEnemy()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            spawnTimer = spawnCoolDown;
            return true;
        }

        return false;
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject choosenEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.Remove(choosenEnemy);
        return choosenEnemy;
    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, transform.position, Quaternion.identity);

        Enemy enemy = newEnemy.GetComponent<Enemy>();
        enemy.SetupEnemyWaypoint(waypoints, this);

        activeEnemies.Add(newEnemy);
    }

    [ContextMenu("Collect waypoints")]
    private void CollectWaypoints()
    {
        // Make a new waypoint list each time
        waypoints = new();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Waypoint>(out var waypoint))
            {
                waypoints.Add(waypoint);
            }
        }
    }

    public List<GameObject> GetActiveEnemies() => activeEnemies;

    public void AddEnemy(GameObject enemy) => enemiesToCreate.Add(enemy);

    public void RemoveActiveEnemy(GameObject enemyToRemove) => activeEnemies.Remove(enemyToRemove);
}
