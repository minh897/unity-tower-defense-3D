using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private float spawnCoolDown;
    [SerializeField] private WaveManager myWaveManager;
    [SerializeField] private List<Waypoint> waypointList;
    [Space]

    [SerializeField] private ParticleSystem flyPortalFX;

    private float spawnTimer;
    private Coroutine flyPortalFXCo;
    private List<GameObject> enemiesToCreate = new();
    private List<GameObject> activeEnemies = new();
    private ObjectPoolManager objectPool;

    public Vector3[] currentWaypoints { get; private set; }

    void Awake()
    {
        CollectWaypoints();

        // if (myWaveManager == null)
        //     myWaveManager = FindFirstObjectByType<LevelSetup>().GetWaveManager();
    }

    void Start()
    {
        objectPool = ObjectPoolManager.instance;
    }

    void Update()
    {
        if (CanMakeNewEnemy())
            SpawnEnemy();
    }

    private void PlaceEnemyAtFlyPortalIfNeeded(GameObject newEnemy, EnemyType enemyType)
    {
        if (enemyType != EnemyType.Flying)
            return;

        if (flyPortalFXCo != null)
            StopCoroutine(flyPortalFXCo);

        flyPortalFXCo = StartCoroutine(PlayFlyPortalFXCo());
        newEnemy.transform.position = flyPortalFX.transform.position;
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

    private void SpawnEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = objectPool.Get(randomEnemy, transform.position, Quaternion.identity);

        Enemy enemy = newEnemy.GetComponent<Enemy>();
        enemy.SetupEnemyWaypoint(this);
        
        PlaceEnemyAtFlyPortalIfNeeded(newEnemy, enemy.GetEnemyType());

        newEnemy.SetActive(true);
        activeEnemies.Add(newEnemy);
    }

    private void CollectWaypoints()
    {
        // Make a new waypoint list each time
        waypointList = new();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Waypoint>(out var waypoint))
                waypointList.Add(waypoint);
        }

        currentWaypoints = new Vector3[waypointList.Count];

        // Collect all waypoints only once instead of on each enemy
        for (int i = 0; i < currentWaypoints.Length; i++)
        {
            currentWaypoints[i] = waypointList[i].transform.position;
        }
    }

    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        activeEnemies.Remove(enemyToRemove);
        // myWaveManager.HandleWaveCompletion();
    }

    private IEnumerator PlayFlyPortalFXCo()
    {
        flyPortalFX.Play();

        yield return new WaitForSeconds(2);

        flyPortalFX.Stop();
    }

    public void AssignWaveManager(WaveManager waveManager) => myWaveManager = waveManager;

    public List<GameObject> GetActiveEnemies() => activeEnemies;

    public void AddEnemy(GameObject enemy) => enemiesToCreate.Add(enemy);

}
