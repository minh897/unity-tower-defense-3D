using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] waypoint;

    private NavMeshAgent agent;
    private int waypointIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     agent.SetDestination(waypoint.position);
    // }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetNextWayPoint());
        }
    }

    private Vector3 GetNextWayPoint()
    {
        if (waypointIndex >= waypoint.Length)
        {
            return transform.position;
        }

        Vector3 targetPosition = waypoint[waypointIndex].position;
        waypointIndex++;
        return targetPosition;
    }
}
