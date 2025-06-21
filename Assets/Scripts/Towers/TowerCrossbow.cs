using UnityEngine;

public class TowerCrossbow : Tower
{
    [Header("Crossbow Details")]
    [SerializeField] private int towerDamage;

    private CrossbowVisual visual;

    protected override void Awake()
    {
        base.Awake();

        visual = GetComponent<CrossbowVisual>();
    }

    protected override void Attack()
    {
        base.Attack();

        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity, whatIsTargetable))
        {
            towerHead.forward = directionToEnemy;

            // Search for IDamagable interface from the hit enemy
            IDamagable damagableIn = hitInfo.transform.GetComponent<IDamagable>();
            damagableIn.TakeDamage(towerDamage);

            visual.CreateOnHitVFX(hitInfo.point);
            visual.PlayAttackVFX(gunPoint.position, hitInfo.point);
            visual.PlayReloadVFX(attackCoolDown);

            AudioManager.instance?.PlaySFX(attackSFX, true);
        }
    }
}
