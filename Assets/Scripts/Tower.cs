using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("TOWER SETUP")]
    [Space]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRadius = 2.5f;
    [SerializeField] protected float attackCoolDown = 1f;

    protected Transform currentEnemy;
    protected Collider[] towerOverlapResults;
    protected int maxEnemyOverlap = 10;
    protected float lastTimeAttacked;

    private bool canRotate;


    protected virtual void Awake()
    {
        towerOverlapResults = new Collider[maxEnemyOverlap];
    }

    protected virtual void Update()
    {
        CheckForEnemies();
        RotateTowardsEnemy();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    protected bool IsEnemyOutOfRange(Transform enemy)
    {
        return Vector3.Distance(enemy.position, transform.position) > attackRadius;
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack start at " + Time.time);
    }

    protected Vector3 DirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.position - startPoint.position).normalized;
    }

    public void EnableRotation(bool isEnable)
    {
        canRotate = isEnable;
    }

    protected bool CanAttack()
    {
        if (currentEnemy == null)
        {
            return false;
        }

        if (Time.time > lastTimeAttacked + attackCoolDown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    protected void CheckForEnemies()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindTheClosestEnemyWithinRange();
            return;
        }

        if (CanAttack())
        {
            Attack();
        }

        // Clear the current enemy if they're out of range
        if (IsEnemyOutOfRange(currentEnemy))
        {
            currentEnemy = null;
            return;
        }
    }

    protected virtual void RotateTowardsEnemy()
    {
        if (canRotate == false)
        {
            return;
        }

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
    }

    // Return the enemy closest enemy withing the tower's attack radius
    // Uses Physics.OverlapSphereNonAlloc for performance
    // Loop through all detected enemies and return the one that is closest to the tower
    protected Transform FindTheClosestEnemyWithinRange()
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
