using UnityEngine;

public class ProjectileCannon : MonoBehaviour
{
    [SerializeField] private float damageRadius;
    [SerializeField] private GameObject VFXExplosion;
    [SerializeField] private LayerMask whatIsEnemy;

    private float projectileDamage;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    public void SetupProjectile(Vector3 newVelocity, float towerDamage)
    {
        rb.linearVelocity = newVelocity;
        projectileDamage = towerDamage;
    }

    public void DamageEnemies()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, damageRadius, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            IDamagable damagableIn = enemy.GetComponent<IDamagable>();

            if (damagableIn != null)
                damagableIn.TakeDamage(Mathf.RoundToInt(projectileDamage));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DamageEnemies();
        VFXExplosion.SetActive(true);
        VFXExplosion.transform.parent = null;
        Destroy(gameObject);
    }
}
