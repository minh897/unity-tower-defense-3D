using UnityEngine;
using UnityEngine.AI;

public enum EnemyType {None, Basic, Fast}

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Transform[] waypoints;

    public int healthPoints = 4;

    private float totalDistance;
    private int waypointIndex;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    void Start()
    {
        waypoints = FindFirstObjectByType<WaypointManager>().GetWaypoints();

        CalculateTotalDistance();
    }

    void Update()
    {
        FaceTarget(agent.steeringTarget);
        SetNextDestination();
    }

    public float CalculateDistanceToGoal() => totalDistance + agent.remainingDistance;

    public Vector3 GetCenterPoint() => centerPoint.position;

    public EnemyType GetEnemyType() => enemyType;

    // Calculate the distance between each waypoint and the add each of them to the total distance
    private void CalculateTotalDistance()
    {
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalDistance += distance;
        }
    }

    // Returns the position of the next waypoint in the sequence.
    // If all waypoints have been reached, it returns the current position instead.
    private Vector3 GetNextWayPoint()
    {
        if (waypointIndex >= waypoints.Length)
        {
            return transform.position;
        }

        Vector3 targetPosition = waypoints[waypointIndex].position;

        // Subtract the distance between the current waypoint and the previous one from the total distance
        // Start from index 1 because index 0 has no previous waypoint (avoid out of bound error)
        if (waypointIndex > 0)
        {
            float distance = Vector3.Distance(waypoints[waypointIndex].position, waypoints[waypointIndex - 1].position);
            totalDistance -= distance;
        }

        waypointIndex++;

        return targetPosition;
    }

    // Set a destination for the next waypoint when the enemy remaining distance is close to the current waypoint
    private void SetNextDestination()
    {
        if (agent.remainingDistance < 0.5f)
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
        {
            Destroy(gameObject);
        }
    }
}
