using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("TOWER SETUP")]
    [Space]
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRadius = 2.5f;
    [SerializeField] protected float attackCoolDown = 1f;
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected LayerMask enemyLayer;

    protected int maxEnemyOverlap = 10;
    protected float lastTimeAttacked;
    protected Transform currentEnemy;
    protected Collider[] enemyOverlapList;

    private bool canRotate;


    protected virtual void Awake()
    {
        enemyOverlapList = new Collider[maxEnemyOverlap];
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
            currentEnemy = FindEnemy();
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

    // Returns the transform of the closest enemy within the tower's attack radius.
    // Uses Physics.OverlapSphereNonAlloc to detect nearby enemies and FindTheClosestEnemy()
    // to determine which one is closest to the finish line.
    protected Transform FindEnemy()
    {
        List<Enemy> enemyList = new();

        // Check for all enemies within attack radius using layer mask, and store them in pre-allocated array
        int enemyNumber = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, enemyOverlapList, enemyLayer);

        for (int i = 0; i < enemyNumber; i++)
        {
            Collider enemyCollider = enemyOverlapList[i];

            // Try to get the component Eney from enemyCollider
            // Return true if it exists and store the result in the variable enemy
            if (enemyCollider.TryGetComponent<Enemy>(out var enemy))
            {
                enemyList.Add(enemy);
            }
        }

        return GetTheClosestEnemy(enemyList);
    }

    protected Transform GetTheClosestEnemy(List<Enemy> enemyList)
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemyList)
        {
            float remainDistance = enemy.CalculateDistanceToGoal();

            // Update if the enemy distance is closer than previous closest distance
            if (remainDistance < closestDistance)
            {
                closestDistance = remainDistance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}
