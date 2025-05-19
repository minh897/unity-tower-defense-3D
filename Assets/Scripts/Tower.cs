using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Setup")]
    [Space]
    [SerializeField] private Transform towerHead;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int maxEnemyOverlap;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackRadius;
    [SerializeField] private Collider[] towerOverlapResults;

    private Transform currentEnemy;

    void Awake()
    {
        towerOverlapResults = new Collider[maxEnemyOverlap];
    }

    void Update()
    {
        CheckForEnemies();
        RotateTowardsEnemy();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void RotateTowardsEnemy()
    {
        if (currentEnemy == null)
        {
            return;
        }

        // Calculate the vector direction from the tower head to the current enemy's position
        Vector3 directionToTarget = currentEnemy.position - towerHead.position;

        // Create a new rotation that look in the direction of the target
        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        // Calculate the in-between rotation from the current to the target orientation
        // Then convert it to Euler angles (rotations in degrees around each axis)
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, newRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Convert the Euler angle rotation to Quaternion and apply it to the tower head rotation 
        towerHead.rotation = Quaternion.Euler(rotation);

        Debug.DrawRay(towerHead.position, directionToTarget, Color.green);
    }

    private bool IsEnemyOutOfRange(Transform enemy)
    {
        return Vector3.Distance(enemy.position, transform.position) > attackRadius;
    }

    private void CheckForEnemies()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindTheClosestEnemyWithinRange();
            return;
        }
        
        // Clear the current enemy if they're out of range
        if (IsEnemyOutOfRange(currentEnemy))
        {
            currentEnemy = null;
            return;
        }
    }

    // Return the enemy closest enemy withing the tower's attack radius
    // Uses Physics.OverlapSphereNonAlloc for performance
    // Loop through all detected enemies and return the one that is closest to the tower
    private Transform FindTheClosestEnemyWithinRange()
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Check for all enemies withing attack radius, and store them in pre-allocated array
        int hits = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, towerOverlapResults, enemyLayer);

        for (int i = 0; i < hits; i++)
        {
            Transform potentialEnemy = towerOverlapResults[i].transform;
            float currentEnemyDistance = Vector3.Distance(transform.position, potentialEnemy.position);

            // Update if the enemy distance is closer than previous closest distance
            if (currentEnemyDistance < closestDistance)
            {
                closestDistance = currentEnemyDistance;
                closestEnemy = potentialEnemy;
            }
        }

        return closestEnemy;
    }
}
