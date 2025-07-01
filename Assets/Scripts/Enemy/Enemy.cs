using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType {None, Basic, Fast, Swarm, Heavy, Stealth, Flying, SpiderBoss}

public class Enemy : MonoBehaviour, IDamagable
{
    public float currentHP = 4;
    public float maxHP = 100;

    public EnemyVisual visual { get; private set; }

    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] protected Vector3[] enemyWaypoints;

    private int originalLayerIndex;
    private float totalDistance;
    private GameManager gameManager;
    private Coroutine hideCo;
    private Coroutine disableHideCo;

    protected bool canBeHidden = true;
    protected bool isHidden = true;
    protected bool isDead;
    protected int nextWaypointIndex;
    protected int currentWavepointIndex;
    protected float originalSpeed;
    protected EnemyPortal enemyPortal;
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected ObjectPoolManager objectPool;

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

        objectPool = ObjectPoolManager.instance;
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

    public void SetupEnemyWaypoint(EnemyPortal referencePortal)
    {
        enemyPortal = referencePortal;

        UpdateWaypoint(enemyPortal.currentWaypoints);
        CalculateTotalDistance();
        ResetEnemy();
        BeginMovement();
    }

    private void BeginMovement()
    {
        currentWavepointIndex = 0;
        nextWaypointIndex = 0;
        ChangeWayPoint();
    }

    protected void ResetEnemy()
    {
        gameObject.layer = originalLayerIndex;
        visual.MakeTransparent(false);
        currentHP = maxHP;
        isDead = false;
        agent.speed = originalSpeed;
        agent.enabled = true;
    }

    private void UpdateWaypoint(Vector3[] newWaypoints)
    {
        enemyWaypoints = new Vector3[newWaypoints.Length];

        for (int i = 0; i < enemyWaypoints.Length; i++)
        {
            enemyWaypoints[i] = newWaypoints[i];
        }
    }

    // Calculate the distance between each waypoint and the add each of them to the total distance
    private void CalculateTotalDistance()
    {
        for (int i = 0; i < enemyWaypoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(enemyWaypoints[i], enemyWaypoints[i + 1]);
            totalDistance += distance;
        }
    }

    protected virtual bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= enemyWaypoints.Length)
            return false;

        if (agent?.remainingDistance <= 0.5f)
            return true;
        
        Vector3 currentWaypoint = enemyWaypoints[currentWavepointIndex];
        Vector3 nextWaypoint = enemyWaypoints[nextWaypointIndex];

        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenWaypoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceToNextWaypoint < distanceBetweenWaypoints;
    }

    protected virtual void ChangeWayPoint()
    {
        if (agent.isOnNavMesh == true)
            agent.SetDestination(GetNextWayPoint());
        else
            Debug.Log("This one is not on Nav Mesh: " + agent.gameObject);
    }

    protected Vector3 GetFinalWayPoint()
    {
        if (enemyWaypoints.Length == 0)
            return transform.position;

        return enemyWaypoints[enemyWaypoints.Length - 1];
    }

    // Returns the position of the next waypoint in the sequence.
    // If all waypoints have been reached, it returns the current position instead.
    private Vector3 GetNextWayPoint()
    {
        if (nextWaypointIndex >= enemyWaypoints.Length)
            return transform.position;

        Vector3 targetPosition = enemyWaypoints[nextWaypointIndex];

        // Subtract the distance between the current waypoint and the previous one from the total distance
        // Start from index 1 because index 0 has no previous waypoint (avoid out of bound error)
        if (nextWaypointIndex > 0)
        {
            float distance = Vector3.Distance(enemyWaypoints[nextWaypointIndex], enemyWaypoints[nextWaypointIndex - 1]);
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
        currentHP -= damage;

        if (rb != null && currentHP <= 0 && isDead == false)
        {
            // Use flag isDead in case Die() is called twice
            isDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        gameManager.UpdateCurrency(1);
        RemoveEnemy();
    }

    public virtual void RemoveEnemy()
    {
        if (visual != null)
            visual.CreateDeathVFX();

        objectPool.Remove(gameObject);

        // Prevent enemy from being too far from nav mesh when disabled
        agent.enabled = false;

        if (enemyPortal != null)
            enemyPortal.RemoveActiveEnemy(gameObject);
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

    public virtual float CalculateDistanceToGoal() => totalDistance + agent.remainingDistance;

    public Vector3 GetCenterPoint() => centerPoint.position;

    public EnemyType GetEnemyType() => enemyType;

}
