using UnityEngine;

public class TowerCrossbow : Tower
{
    [Header("Crossbow Details")]
    [SerializeField] private Transform gunPoint;

    private CrossbowVisual visual;

    protected override void Awake()
    {
        base.Awake();

        visual = GetComponent<CrossbowVisual>();
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Debug.DrawLine(gunPoint.position, hitInfo.point);
            Debug.Log(hitInfo.collider.gameObject.name + " was attacked");
            
            visual.PlayAttackFX(gunPoint.position, hitInfo.point);
            visual.PlayReloadFX(attackCoolDown);
        }
    }
}
