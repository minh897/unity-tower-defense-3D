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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = FindFirstObjectByType<WaypointManager>().GetWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget(agent.steeringTarget);
        
        // Check if the agent is close to current target point
        // Then set a destination for the next waypoint
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetNextWayPoint());
        }
    }

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

    private void FaceTarget(Vector3 newTarget)
    {
        // Calculate the direction from current position to next target
        // Draw a vector from point A to point B essentially
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
