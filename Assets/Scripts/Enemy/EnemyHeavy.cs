using UnityEngine;

public class EnemyHeavy : Enemy
{
    [Header("Enemy Details")]
    [SerializeField] private int shieldAmount = 50;
    [SerializeField] private EnemyShield shieldObject;

    protected override void Awake()
    {
        base.Awake();

        if (shieldObject != null)
        {
            shieldObject.SetupShield(shieldAmount);
            shieldObject.gameObject.SetActive(true);
        }
    }
}
