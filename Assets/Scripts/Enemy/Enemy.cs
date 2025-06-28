using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType {None, Basic, Fast, Swarm, Heavy, Stealth, Flying, SpiderBoss}

public class Enemy : MonoBehaviour, IDamagable
{
    public float healthPoints = 4;

    public EnemyVisual visual { get; private set; }

    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] protected List<Transform> enemyWaypoints;

    private int originalLayerIndex;
    private float totalDistance;
    private GameManager gameManager;
    private Coroutine hideCo;
    private Coroutine disableHideCo;

    protected bool canBeHidden = true;
    protected bool isHidden = true;
    protected bool isDead; // set to false later
    protected int nextWaypointIndex;
    protected int currentWavepointIndex;
    protected float originalSpeed;
    protected EnemyPortal enemyPortal;
    protected NavMeshAgent agent;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);

        visual = GetComponent<EnemyVisual>();
        originalLayerIndex = gameObject.layer;

        gameManager = FindFirstObjectByType<GameManager>();
        originalSpeed = agent.speed;
    }

    protected virtual void Start()
    {
        // This is just for override
    }

    protected virtual void Update()
    {
        FaceTarget(agent.steeringTarget);
        SetNextDestination();
    }

    public void SlowEnemy(float slowMultiplier, float duration)
    {
        StartCoroutine(SlowEnemyCo(slowMultiplier, duration));
    }

    private IEnumerator SlowEnemyCo(float slowMultiplier, float duration)
    {
        agent.speed = originalSpeed;
        agent.speed *= slowMultiplier;

        yield return new WaitForSeconds(duration);

        agent.speed = originalSpeed;
    }

    public void DisableHide(float duration)
    {
        if (disableHideCo != null)
            StopCoroutine(disableHideCo);
        
        disableHideCo = StartCoroutine(DisableHideCo(duration));
    }

    protected virtual IEnumerator DisableHideCo(float duration)
    {
        canBeHidden = false;
        yield return new WaitForSeconds(duration);
        canBeHidden = true;
    } 

    public void HideEnemy(float duration)
    {
        if (canBeHidden == false)
            return;

        if (hideCo != null)
            StopCoroutine(hideCo);

        hideCo = StartCoroutine(HideEnemyCo(duration));
    }

    private IEnumerator HideEnemyCo(float duration)
    {
        gameObject.layer = LayerMask.NameToLayer("Untargetable");
        visual.MakeTransparent(true);
        isHidden = true;

        yield return new WaitForSeconds(duration);

        gameObject.layer = originalLayerIndex;
        visual.MakeTransparent(false);
        isHidden = false;
    }

    public void SetupEnemyWaypoint(List<Waypoint> newEnemyWaypoints, EnemyPortal referencePortal)
    {
        enemyWaypoints = new();

        foreach (var point in newEnemyWaypoints)
        {
            enemyWaypoints.Add(point.transform);
        }

        CalculateTotalDistance();

        enemyPortal = referencePortal;
    }

    // Calculate the distance between each waypoint and the add each of them to the total distance
    private void CalculateTotalDistance()
    {
        for (int i = 0; i < enemyWaypoints.Count - 1; i++)
        {
            float distance = Vector3.Distance(enemyWaypoints[i].position, enemyWaypoints[i + 1].position);
            totalDistance += distance;
        }
    }

    protected virtual bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= enemyWaypoints.Count)
            return false;

        if (agent.remainingDistance <= 0.5f)
            return true;
        
        Vector3 currentWaypoint = enemyWaypoints[currentWavepointIndex].position;
        Vector3 nextWaypoint = enemyWaypoints[nextWaypointIndex].position;

        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenWaypoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceToNextWaypoint < distanceBetweenWaypoints;
    }

    protected virtual void ChangeWayPoint()
    {
        agent.SetDestination(GetNextWayPoint());
    }

    protected Vector3 GetFinalWayPoint()
    {
        if (enemyWaypoints.Count == 0)
            return transform.position;

        return enemyWaypoints[enemyWaypoints.Count - 1].position;
    }

    // Returns the position of the next waypoint in the sequence.
    // If all waypoints have been reached, it returns the current position instead.
    private Vector3 GetNextWayPoint()
    {
        if (nextWaypointIndex >= enemyWaypoints.Count)
            return transform.position;

        Vector3 targetPosition = enemyWaypoints[nextWaypointIndex].position;

        // Subtract the distance between the current waypoint and the previous one from the total distance
        // Start from index 1 because index 0 has no previous waypoint (avoid out of bound error)
        if (nextWaypointIndex > 0)
        {
            float distance = Vector3.Distance(enemyWaypoints[nextWaypointIndex].position, enemyWaypoints[nextWaypointIndex - 1].position);
            totalDistance -= distance;
        }

        nextWaypointIndex++;

        currentWavepointIndex = nextWaypointIndex - 1;

        return targetPosition;
    }

    // Set a destination for the next waypoint when the enemy remaining distance is close to the current waypoint
    private void SetNextDestination()
    {
        if (ShouldChangeWaypoint())
            ChangeWayPoint();
    }

    // Smoothly rotate the enemy game object to face the given target position
    private void FaceTarget(Vector3 newTarget)
    {
        // Calculate the direction from current position to next target
        Vector3 directionToTarget = newTarget - transform.position;

        // Ignore any difference in vertical position
        directionToTarget.y = 0;

        // Create a new rotation that points the forward vector (y) of the game object up to the calculated direction
        // Since we ignore the vertical position above, the game object can only rotate horizontally
        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        // Create a smooth rotation from the current rotation to the target rotation at a defined speed
        // Time.deltaTime makes this framerate independent
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(float damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0 && isDead == false)
        {
            // Use flag isDead in case Die() is called twice
            isDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        gameManager.UpdateCurrency(1);
        DestroyEnemy();
    }

    public virtual void DestroyEnemy()
    {
        visual.CreateDeathVFX();
        Destroy(gameObject);

        if (enemyPortal != null)
            enemyPortal.RemoveActiveEnemy(gameObject);
    }

    public virtual float CalculateDistanceToGoal() => totalDistance + agent.remainingDistance;

    public Vector3 GetCenterPoint() => centerPoint.position;

    public EnemyType GetEnemyType() => enemyType;

}
