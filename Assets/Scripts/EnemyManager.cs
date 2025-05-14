using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;

    [Header("Respawn Point")]
    [SerializeField] private Transform respawn;
    
    private float spawnCoolDown;
    private float spawnTimer;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            CreateEnemy();
            spawnTimer = spawnCoolDown;
        }
    }

    private void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(basicEnemy, respawn.position, Quaternion.identity);
    }
}
