using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType {None, Basic, Fast}

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private List<Transform> enemyWaypoints;

    public int healthPoints = 4;

    private int nextWaypointIndex;
    private int currentWavepointIndex;
    private float totalDistance;
    private NavMeshAgent agent;
    private EnemyPortal enemyPortal;
    private GameManager gameManager;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindFirstObjectByType<GameManager>();

        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    void Start()
    {
        CalculateTotalDistance();
    }

    void Update()
    {
        FaceTarget(agent.steeringTarget);
        SetNextDestination();
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

    private bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= enemyWaypoints.Count)
        {
            return false;
        }

        if (agent.remainingDistance <= 0.1f)
        {
            return true;
        }
        
        Vector3 currentWaypoint = enemyWaypoints[currentWavepointIndex].position;
        Vector3 nextWaypoint = enemyWaypoints[nextWaypointIndex].position;

        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenWaypoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceToNextWaypoint < distanceBetweenWaypoints;
    }

    // Returns the position of the next waypoint in the sequence.
    // If all waypoints have been reached, it returns the current position instead.
    private Vector3 GetNextWayPoint()
    {
        if (nextWaypointIndex >= enemyWaypoints.Count)
        {
            return transform.position;
        }

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
        {
            agent.SetDestination(GetNextWayPoint());
        }
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

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
            Die();
    }

    private void Die()
    {
        enemyPortal.RemoveActiveEnemy(gameObject);
        gameManager.UpdateCurrency(1);
        Destroy(gameObject);
    }

    public void DestroyEnemy()
    {
        enemyPortal.RemoveActiveEnemy(gameObject);
        Destroy(gameObject);
    }

    public float CalculateDistanceToGoal() => totalDistance + agent.remainingDistance;

    public Vector3 GetCenterPoint() => centerPoint.position;

    public EnemyType GetEnemyType() => enemyType;

}
