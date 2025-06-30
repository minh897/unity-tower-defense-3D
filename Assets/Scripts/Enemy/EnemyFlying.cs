using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : Enemy
{
    private List<TowerHarpoon> observingTowers = new();

    protected override void Start()
    {
        base.Start();

        agent.SetDestination(GetFinalWayPoint());
    }

    public override float CalculateDistanceToGoal()
    {
        return Vector3.Distance(transform.position, GetFinalWayPoint());
    }

    public override void DestroyEnemy()
    {
        foreach (var tower in observingTowers)
            tower.ResetAttack();

        foreach (var harpoon in GetComponentsInChildren<ProjectileCannon>())
            objectPool.Remove(harpoon.gameObject);

        base.DestroyEnemy();
    }

    public void AddObservingTower(TowerHarpoon newTower) => observingTowers.Add(newTower);
}
