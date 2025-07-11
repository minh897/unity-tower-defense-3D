using UnityEngine;

public class EnemyHeavy : Enemy
{
    [Header("Enemy Details")]
    [SerializeField] private float currentShield = 50;
    [SerializeField] private float maxShield = 150;
    [SerializeField] private EnemyShield shieldObject;

    protected override void OnEnable()
    {
        base.OnEnable();

        currentShield = maxShield;
        EnableShieldIfNeeded();
    }

    private void EnableShieldIfNeeded()
    {
        if (shieldObject != null && currentShield > 0)
            shieldObject.gameObject.SetActive(true);
    }

    public override void TakeDamage(float damage)
    {
        if (currentShield <= 0)
            base.TakeDamage(damage);
        else
        {
            currentShield -= damage;
            shieldObject.ActivateShieldImpact();

            if (currentShield <= 0)
                shieldObject.gameObject.SetActive(false);
        }
    }
}
