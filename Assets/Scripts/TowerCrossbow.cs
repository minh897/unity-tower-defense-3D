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

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;

            // Play both attack visual and reload crossbow visual
            visual.PlayAttackFX(gunPoint.position, hitInfo.point);
            visual.PlayReloadFX(attackCoolDown);

            // Search for IDamagable interface from the hit enemy
            IDamagable damgableInterface = hitInfo.transform.gameObject.GetComponent<IDamagable>();

            if (damgableInterface != null)
            {
                damgableInterface.TakeDamage(towerDamage);
            }
        }
    }
}
