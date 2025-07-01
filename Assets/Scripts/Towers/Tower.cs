using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Enemy currentEnemy;

    [SerializeField] protected float attackCoolDown = 1f;
    [Space]

    [Header("Tower Setup")]
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected Transform towerBody;
    [SerializeField] protected Transform gunPoint;
    [SerializeField] protected EnemyType enemyPriorityType;
    public bool isAttackForward;
    [Space]

    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected LayerMask whatIsTargetable;
    [Space]

    [Header("SFX Details")]
    [SerializeField] protected AudioSource attackSFX;

    protected bool isTowerActive = true;
    protected float lastTimeAttacked;
    protected Coroutine deactiveTowerCo;
    protected ObjectPoolManager objectPool;
    protected Collider[] enemyHitColliders = new Collider[50];

    private GameObject currentEMPFX;

    protected virtual void Awake()
    {
        // For override
    }

    protected virtual void Start()
    {
        objectPool = ObjectPoolManager.instance;
    }

    protected virtual void FixedUpdate()
    {
        CheckForEnemies();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void DeactivateTower(float duration, GameObject empFX)
    {
        if (deactiveTowerCo != null)
            StopCoroutine(deactiveTowerCo);

        if (currentEMPFX != null)
            objectPool.Remove(currentEMPFX);

        currentEMPFX = objectPool.Get(empFX, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        deactiveTowerCo = StartCoroutine(DeactivateTowerCo(duration));
    }

    protected void CheckForEnemies()
    {
        if (isTowerActive == false)
            return;

        LoseTargetIfNeeded();
        UpdateTargetIfNeeded();
        HandleRotation();

        if (CanAttack())
            AttemptToAttack();
    }

    private void UpdateTargetIfNeeded()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindEnemiesWithinRange();
            return;
        }
    }

    protected virtual void HandleRotation()
    {
        RotateTowardsEnemy();
        RotateBodyTowardsEnemy();
    }

    protected virtual void RotateTowardsEnemy()
    {
        if (currentEnemy == null || towerHead == null)
            return;

        // Calculate the vector direction from the tower head to the current enemy's position
        Vector3 directionToTarget = DirectionToEnemyFrom(towerHead);

        // Create a new rotation that look in the direction of the target
        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        // Calculate the in-between rotation from the current to the target orientation
        // Then convert it to Euler angles (rotations in degrees around each axis)
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, newRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Convert the Euler angle rotation to Quaternion and apply it to the tower head rotation 
        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void RotateBodyTowardsEnemy()
    {
        if (towerBody == null|| currentEnemy == null)
            return;

        Vector3 directionToEnemy = DirectionToEnemyFrom(towerBody);
        directionToEnemy.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
        towerBody.rotation = Quaternion.Slerp(towerBody.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    // Tower can still target disabled enemy from object pool
    // Make a check for inactive current enemy so it can switch to other enemy
    protected void AttemptToAttack()
    {
        if (currentEnemy != null && currentEnemy.gameObject.activeSelf == false)
        {
            currentEnemy = null;
            return;
        }

        Attack();
    }

    // Returns the transform of the closest enemy within the tower's attack radius.
    // Uses Physics.OverlapSphereNonAlloc to detect nearby enemies and FindTheClosestEnemy()
    // to determine which one is closest to the finish line.
    protected virtual Enemy FindEnemiesWithinRange()
    {
        List<Enemy> allEnemy = new();
        List<Enemy> priorityEnemies = new();

        // Check for all enemies within attack radius using layer mask, and store them in pre-allocated array
        int enemycount = Physics.OverlapSphereNonAlloc(transform.position, attackRange, enemyHitColliders, whatIsEnemy);

        for (int i = 0; i < enemycount; i++)
        {
            if (!enemyHitColliders[i].TryGetComponent<Enemy>(out var enemy))
                continue;

            if (enemy.GetEnemyType() == enemyPriorityType)
                priorityEnemies.Add(enemy);
            else
                allEnemy.Add(enemy);
        }

        if (priorityEnemies.Count > 0)
            return GetTheClosestEnemy(priorityEnemies);

        if (allEnemy.Count > 0)
            return GetTheClosestEnemy(allEnemy);

        return null;
    }

    private Enemy GetTheClosestEnemy(List<Enemy> enemyList)
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemyList)
        {
            float remainDistance = enemy.CalculateDistanceToGoal();

            // Update if the enemy distance is closer than previous closest distance
            if (remainDistance < closestDistance)
            {
                closestDistance = remainDistance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private IEnumerator DeactivateTowerCo(float duration)
    {
        isTowerActive = false;
        yield return new WaitForSeconds(duration);
        isTowerActive = true;

        // Prevent the tower from attacking immediately when enable
        // Let the tower has time to adjust their head position before attacking
        lastTimeAttacked = Time.time;
        objectPool.Remove(currentEMPFX);
    }

    protected virtual void LoseTargetIfNeeded()
    {
        if (currentEnemy == null)
            return;

        if (Vector3.Distance(currentEnemy.transform.position, transform.position) > attackRange)
            currentEnemy = null;
    }

    public float GetAttackRange() => attackRange;

    public float GetAttackCooldown() => attackCoolDown;

    protected virtual void Attack() => lastTimeAttacked = Time.time;

    protected virtual bool CanAttack() => Time.time > lastTimeAttacked + attackCoolDown && currentEnemy != null;

    protected virtual Vector3 DirectionToEnemyFrom(Transform startPoint) => (currentEnemy.GetCenterPoint() - startPoint.position).normalized;

}
