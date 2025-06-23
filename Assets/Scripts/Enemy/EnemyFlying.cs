using UnityEngine;

public class EnemyFlying : Enemy
{
    protected override void Start()
    {
        base.Start();

        agent.SetDestination(GetFinalWayPoint());
    }

    public override float CalculateDistanceToGoal()
    {
        return Vector3.Distance(transform.position, GetFinalWayPoint());
    }
}
