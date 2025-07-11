using UnityEngine;

public class EnemyBossUnit : Enemy
{
    private Vector3 savedDestination;
    private Vector3 lastKnownBossPosition;
    private EnemyFlyingBoss myBoss;

    protected override void Update()
    {
        base.Update();

        if (myBoss != null)
            lastKnownBossPosition = myBoss.transform.position;
    }

    public void SetupEnemy(Vector3 destination, EnemyFlyingBoss myNewBoss, EnemyPortal myNewPortal)
    {
        ResetEnemy();
        ResetMovement();

        myBoss = myNewBoss;
        enemyPortal = myNewPortal;
        enemyPortal.GetActiveEnemies().Add(gameObject);
        savedDestination = destination;

        InvokeRepeating(nameof(SnapToBossIfNeeded), .1f, .5f);
    }

    private void ResetMovement()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        agent.enabled = false;
    }

    // Disable gravity and enable kinematic once spawned unit touch the ground, then set its destination
    // The gravity is enabled and kinematic is disabled by default to let the unit fall to the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            return;

        if (Vector3.Distance(transform.position, lastKnownBossPosition) > 2.5f)
        {
            if (myBoss != null)
                transform.position = lastKnownBossPosition + new Vector3(0, -1, 0);
        }

        rb.useGravity = false;
        rb.isKinematic = true;

        agent.enabled = true;
        agent.SetDestination(savedDestination);
    }

    private void SnapToBossIfNeeded()
    {
        if (agent.enabled && agent.isOnNavMesh == false)
        {
            if (Vector3.Distance(transform.position, lastKnownBossPosition) > 3f)
            {
                transform.position = lastKnownBossPosition + new Vector3(0, -1, 0);
                ResetMovement();
            }
        }
    }

    public override float CalculateDistanceToGoal()
    {
        return Vector3.Distance(transform.position, GetFinalWayPoint());
    }
}
