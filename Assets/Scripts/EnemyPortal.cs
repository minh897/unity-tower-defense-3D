using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [Header("Respawn Details")]
    [SerializeField] private float spawnCoolDown;

    private float spawnTimer;
    public List<GameObject> enemiesToCreate;
    


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
    }


    public List<GameObject> GetEnemyList() => enemiesToCreate;
}
