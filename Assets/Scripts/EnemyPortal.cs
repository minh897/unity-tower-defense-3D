using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [Header("Respawn Details")]
    [SerializeField] private float spawnCoolDown;

    public List<GameObject> enemiesToCreate;
    public List<Waypoint> waypoints;

    private float spawnTimer;

    void Awake()
    {
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
        enemy.SetupEnemyWaypoint(waypoints);
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


    public List<GameObject> GetEnemyList() => enemiesToCreate;
}
