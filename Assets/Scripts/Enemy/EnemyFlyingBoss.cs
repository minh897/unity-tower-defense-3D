using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFlyingBoss : EnemyFlying
{
    [SerializeField] private int amountToCreate = 150;
    [SerializeField] private float cooldown = .2f;
    [SerializeField] private GameObject bossUnitPrefab;

    private int unitsCreated;
    private float creationTimer;
    private EnemyFlyingBoss enemyFlyingBoss;
    private List<Enemy> createdEnemies = new();


    protected override void Update()
    {
        base.Update();

        creationTimer -= Time.deltaTime;

        if (creationTimer < 0 && unitsCreated < amountToCreate)
        {
            creationTimer = cooldown;
            CreateNewBossUnit();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        unitsCreated = 0;
    }

    private void CreateNewBossUnit()
    {
        unitsCreated++;
        GameObject newUnit = objectPool.Get(bossUnitPrefab, transform.position, Quaternion.identity);

        EnemyBossUnit bossUnit = newUnit.GetComponent<EnemyBossUnit>();
        bossUnit.SetupEnemy(GetFinalWayPoint(), this, enemyPortal);

        createdEnemies.Add(bossUnit);
    }

    private void EleminateAllUnits()
    {
        foreach (var enemy in createdEnemies)
        {
            enemy.Die();
        }
    }

    public override void Die()
    {
        EleminateAllUnits();
        base.Die();
    }
}
