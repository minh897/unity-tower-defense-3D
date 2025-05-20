using UnityEngine;

public class TowerCrossbow : Tower
{
    [Header("Crossbow Details")]
    [SerializeField] private Transform gunPoint;


    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Debug.DrawLine(gunPoint.position, hitInfo.point);
            Debug.Log(hitInfo.collider.gameObject.name + " was attacked");
        }
    }
}
