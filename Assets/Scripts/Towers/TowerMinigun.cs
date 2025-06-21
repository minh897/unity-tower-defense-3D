using UnityEngine;

public class TowerMinigun : Tower
{
    [Header("Minigun Details")]
    [SerializeField] private float damage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectilePrefab;
    private MinigunVisual minigunVisual;
    [Space]

    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Transform[] gunPointSet;
    private int gunPointIndex;

    protected override void Awake()
    {
        base.Awake();
        minigunVisual = GetComponent<MinigunVisual>();
    }

    protected override void Attack()
    {
        gunPoint = gunPointSet[gunPointIndex];
        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity, whatIsTargetable))
        {
            IDamagable damagable = hitInfo.transform.GetComponent<IDamagable>();

            if (damagable == null)
                return;

            GameObject newProjectile = Instantiate(projectilePrefab, gunPoint.position, Quaternion.identity);
            newProjectile.GetComponent<ProjectileMinigun>().SetupProjectile(hitInfo.point, damagable, damage, projectileSpeed);

            minigunVisual.ReCoilGun(gunPoint);

            // Set attack on cooldown
            base.Attack();

            // Increase the index and wrap back to 0 using modulo
            gunPointIndex = (gunPointIndex + 1) % gunPointSet.Length;
        }
    }

    protected override void RotateTowardsEnemy()
    {
        if (currentEnemy == null)
            return;

        Vector3 directionToEnemy = currentEnemy.GetCenterPoint() - rotationOffset - towerHead.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);

        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;
        towerHead.rotation = Quaternion.Euler(rotation);
    }
}
