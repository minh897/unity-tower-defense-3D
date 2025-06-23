using UnityEngine;

public class TowerHarpoon : Tower
{
    [Header("Harpoon Details")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileTransform;
    private ProjectileHarpoon currentProjectile;

    protected override void Awake()
    {
        base.Awake();
        CreateNewProjectile();
    }

    protected override void Attack()
    {
        base.Attack();

        if (Physics.Raycast(gunPoint.position, gunPoint.forward, out RaycastHit hitInfo, Mathf.Infinity, whatIsTargetable))
        {
            currentProjectile.SetupProjectile(currentEnemy, projectileSpeed);
        }
    }

    private void CreateNewProjectile()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, projectileTransform.position, projectileTransform.rotation, towerHead);
        currentProjectile = newProjectile.GetComponent<ProjectileHarpoon>();
    }
}
