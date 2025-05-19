using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float turnSpeed = 10f;

    private NavMeshAgent agent;
    private int waypointIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    void Start()
    {
        waypoints = FindFirstObjectByType<WaypointManager>().GetWaypoints();
    }

    void Update()
    {
        FaceTarget(agent.steeringTarget);    
        SetNextDestination();
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

        waypointIndex++;

        return targetPosition;
    }

    // Check if the agent is close to current target point
    // Then set a destination for the next waypoint
    private void SetNextDestination()
    {
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetNextWayPoint());
        }
    }

    // Rotate the game object to face the given target position smoothly
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
}
