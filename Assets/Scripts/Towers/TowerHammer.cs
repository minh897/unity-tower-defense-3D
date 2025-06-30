using System.Collections.Generic;
using UnityEngine;

public class TowerHammer : Tower
{
    [Header("Slow Modifiers")]
    [Range(0, 1)]
    [SerializeField] private float slowMultipler = .4f;
    [SerializeField] private float slowDuration;

    private HammerVisual hammerVisual;

    protected override void Awake()
    {
        base.Awake();

        hammerVisual = GetComponent<HammerVisual>();
    }

    protected override void FixedUpdate()
    {
        if (isTowerActive == false)
            return;

        if (CanAttack())
            Attack();
    }

    protected override void Attack()
    {
        base.Attack();
        hammerVisual.PlayAttackAnimation();

        foreach (var enemy in FindValidTargets())
        {
            enemy.SlowEnemy(slowMultipler, slowDuration);
        }
    }

    protected override bool CanAttack()
    {
        return Time.time > lastTimeAttacked + attackCoolDown && AtLeastOneEnemyInRadius();
    }

    private List<Enemy> FindValidTargets()
    {
        List<Enemy> targets = new();
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();

            if (newEnemy != null)
                targets.Add(newEnemy);
        }

        return targets;
    }

    private bool AtLeastOneEnemyInRadius()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);
        return enemyColliders.Length > 0;
    }
}
