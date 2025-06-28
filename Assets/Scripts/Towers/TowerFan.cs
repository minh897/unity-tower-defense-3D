using System.Collections.Generic;
using UnityEngine;

public class TowerFan : Tower
{
    [Header("Fan Details")]
    [SerializeField] private float revealFrequency = .1f;
    [SerializeField] private float revealDuration = 1f;

    private List<Enemy> enemiesToReveal = new();


    protected override void Awake()
    {
        base.Awake();

        InvokeRepeating(nameof(RevealEnemies), .1f, revealFrequency);
    }

    private void RevealEnemies()
    {
        foreach (Enemy enemy in enemiesToReveal)
        {
            enemy.DisableHide(revealDuration);
        }
    }

    public void AddEnemyToReveal(Enemy enemy) => enemiesToReveal.Add(enemy);

    public void RemoveEnemyToReveal(Enemy enemy) => enemiesToReveal.Remove(enemy);
}
