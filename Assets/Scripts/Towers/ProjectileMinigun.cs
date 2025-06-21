using UnityEngine;

public class ProjectileMinigun : MonoBehaviour
{
    [SerializeField] private GameObject onHitVFX;

    private float damage;
    private float speed;
    private bool isActive = true;
    private IDamagable damagable;
    private Vector3 target;

    void Update()
    {
        if (isActive == false)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) <= 0.01f)
        {
            // Set flag here so it can only activate once
            isActive = false;
            
            damagable.TakeDamage(Mathf.RoundToInt(damage));
            onHitVFX.SetActive(true);

            // Wait for the vfx to finish
            Destroy(gameObject, 1);
        }   
    }

    public void SetupProjectile(Vector3 targetPosition, IDamagable newDamagable, float newDamage, float newSpeed)
    {
        target = targetPosition;
        damagable = newDamagable;
        damage = newDamage;
        speed = newSpeed;
    }
}
