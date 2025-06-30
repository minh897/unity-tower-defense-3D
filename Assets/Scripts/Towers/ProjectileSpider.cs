using UnityEngine;
using UnityEngine.AI;

public class ProjectileSpider : MonoBehaviour
{
    

    [SerializeField] private float damage;
    [SerializeField] private float damageRadius;
    [SerializeField] private float detonateDistance;
    [SerializeField] private GameObject explosionVFX;
    [Space]

    [SerializeField] private float enemyCheckRadius = 10;
    [SerializeField] private float targetUpdateInterval = .5f;
    [SerializeField] private LayerMask whatIsEnemy;

    private NavMeshAgent agent;
    private Transform currentTarget;
    private ObjectPoolManager objectPool;
    private TrailRenderer trail;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        objectPool = ObjectPoolManager.instance;
        trail = GetComponent<TrailRenderer>();

        InvokeRepeating(nameof(UpdateClosestTarget), .1f, targetUpdateInterval);
    }

    void Update()
    {
        if (currentTarget == null || agent.enabled == false || agent.isOnNavMesh == false)
            return;

        agent.SetDestination(currentTarget.position);

        if (Vector3.Distance(transform.position, currentTarget.position) < detonateDistance)
            Explode();
    }

    public void Explode()
    {
        DamageEnemies();
        objectPool.Get(explosionVFX, transform.position + new Vector3(0, 0.4f, 0));
        objectPool.Remove(gameObject);
    }

    public void DamageEnemies()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, damageRadius, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            IDamagable damagableIn = enemy.GetComponent<IDamagable>();

            if (damagableIn != null)
                damagableIn.TakeDamage(damage);
        }
    }

    public void SetupSpider(float towerDamage)
    {
        trail.Clear();
        damage = towerDamage;
        agent.enabled = true;
        transform.parent = null;
    }

    private void UpdateClosestTarget()
    {
        currentTarget = FindClosestEnemy();
    }

    private Transform FindClosestEnemy()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, enemyCheckRadius, whatIsEnemy);

        Transform nearestEnemy = null;
        float shortestDistance = float.MaxValue;

        foreach (Collider enemy in enemiesAround)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < shortestDistance)
            {
                nearestEnemy = enemy.transform;
                shortestDistance = distance;
            }
        }

        return nearestEnemy;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius); 
    }
}
