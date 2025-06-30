using UnityEngine;

public class ProjectileCannon : MonoBehaviour
{
    [SerializeField] private float damageRadius;
    [SerializeField] private GameObject VFXExplosion;
    [SerializeField] private LayerMask whatIsEnemy;

    private float projectileDamage;
    private Rigidbody rb;
    private ObjectPoolManager objPool;
    private TrailRenderer trail;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    public void SetupProjectile(Vector3 newVelocity, float towerDamage, ObjectPoolManager newObjPool)
    {
        trail.Clear();
        rb.linearVelocity = newVelocity;
        projectileDamage = towerDamage;
        objPool = newObjPool;
    }

    public void DamageEnemies()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, damageRadius, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            IDamagable damagableIn = enemy.GetComponent<IDamagable>();

            if (damagableIn != null)
                damagableIn.TakeDamage(projectileDamage);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DamageEnemies();

        objPool.Get(VFXExplosion, transform.position);
        objPool.Remove(gameObject);
    }
}
