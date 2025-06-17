using UnityEngine;

public class TowerCrossbow : Tower
{
    [Header("Crossbow Details")]
    [SerializeField] private int towerDamage;
    [SerializeField] private Transform gunPoint;

    private CrossbowVisual visual;

    protected override void Awake()
    {
        base.Awake();

        visual = GetComponent<CrossbowVisual>();

        EnableRotation(true);
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity, whatIsTargetable))
        {
            towerHead.forward = directionToEnemy;

            // Search for IDamagable interface from the hit enemy
            IDamagable damagableIn = hitInfo.transform.gameObject.GetComponent<IDamagable>();
            EnemyShield enemyShield = hitInfo.collider.gameObject.GetComponent<EnemyShield>();

            if (damagableIn != null && enemyShield == null)
            {
                damagableIn.TakeDamage(towerDamage);
            }

            // Damage the shield instead if the enemy has EnemyShield component
            if (enemyShield != null)
            {
                damagableIn = enemyShield.GetComponent<IDamagable>();
                damagableIn.TakeDamage(towerDamage);
            }

            visual.CreateOnHitFX(hitInfo.point);
            visual.PlayAttackFX(gunPoint.position, hitInfo.point);
            visual.PlayReloadFX(attackCoolDown);

            AudioManager.instance?.PlaySFX(attackSFX, true);
        }
    }
}
