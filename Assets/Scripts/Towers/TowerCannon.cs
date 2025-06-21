using UnityEngine;

public class TowerCannon : Tower
{
    [Header("Cannon Details")]
    [SerializeField] private float towerDamage;
    [SerializeField] private float timeToTarget = 1.5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private ParticleSystem attackVFX;

    protected override void Attack()
    {
        base.Attack();

        Vector3 velocity = CalculateLaunchVelocity();
        attackVFX.Play();
        
        // GameObject projectile = Instantiate(projectilePrefab, gunPoint.position, Quaternion.identity);
        GameObject projectile = Instantiate(projectilePrefab, gunPoint.position, Quaternion.identity);
        ProjectileCannon projectileComp = projectile.GetComponent<ProjectileCannon>();

        projectileComp.SetupProjectile(velocity, towerDamage);
    }

    protected override void HandleRotation()
    {
        if (currentEnemy == null)
            return;

        RotateBodyTowardsEnemy();
        FaceLaunchDirection();
    }

    // Override find enemies logic to only target enemy that has the most enemies around it
    protected override Enemy FindEnemiesWithinRange()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        Enemy bestTarget = null;
        int maxNearbyEnemies = 0;

        foreach (Collider enemy in enemiesAround)
        {
            int enemiesAmount = EnemiesAroundEnemy(enemy.transform);

            if (enemiesAmount > maxNearbyEnemies)
            {
                maxNearbyEnemies = enemiesAmount;
                bestTarget = enemy.GetComponent<Enemy>();
            }
        }

        return bestTarget;
    }

    // Return the collider of enemies around targeted enemy
    private int EnemiesAroundEnemy(Transform enemyToCheck)
    {
        Collider[] enemiesAround = Physics.OverlapSphere(enemyToCheck.position, 1, whatIsEnemy);

        return enemiesAround.Length;
    }

    private void RotateBodyTowardsEnemy()
    {
        if (towerBody == null)
            return;

        Vector3 directionToEnemy = DirectionToEnemyFrom(towerBody);
        directionToEnemy.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
        towerBody.rotation = Quaternion.Slerp(towerBody.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    // Rotate tower head towards the calculated launch velocity
    private void FaceLaunchDirection()
    {
        Vector3 attackDirection = CalculateLaunchVelocity();
        Quaternion lookRotation = Quaternion.LookRotation(attackDirection);

        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        towerHead.rotation = Quaternion.Euler(rotation.x, towerHead.eulerAngles.y, 0);
    }

    // Calculate projectile launch velocity
    private Vector3 CalculateLaunchVelocity()
    {
        Vector3 direction = currentEnemy.GetCenterPoint() - gunPoint.position;
        Vector3 directionXZ = new(direction.x, 0, direction.z);

        // Get the speed needed on x and z axis to reach the target in time
        Vector3 velocityXZ = directionXZ / timeToTarget;

        // Calculate the upward velocity so the projectile can reach its target height in time until gravity pull it down 
        float yVelocity = (direction.y - Physics.gravity.y * Mathf.Pow(timeToTarget, 2) / 2) / timeToTarget;
        Vector3 launchVelocity = velocityXZ + (Vector3.up * yVelocity);
        
        return launchVelocity;
    }
}
