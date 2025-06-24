using System.Collections;
using UnityEngine;

public class TowerHarpoon : Tower
{
    [Header("Harpoon Details")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileTransform;
    private ProjectileHarpoon currentProjectile;

    [Header("Damage Details")]
    [SerializeField] private float initialDamage = 5;
    [SerializeField] private float damageOvertime = 10;
    [SerializeField] private float overtimeEffectDuration = 10;
    [Range(0f, 1f)]
    [SerializeField] private float slowEffect = .7f;

    private bool busyWithAttack;
    private Coroutine damageOvertimeCo;

    protected override void Awake()
    {
        base.Awake();
        CreateNewProjectile();
    }

    protected override void Attack()
    {
        base.Attack();

        if (Physics.Raycast(gunPoint.position, gunPoint.forward, out RaycastHit hitInfo, Mathf.Infinity, whatIsTargetable))
        {
            busyWithAttack = true;
            currentProjectile.SetupProjectile(currentEnemy, projectileSpeed, this);
        }
    }

    public void ResetAttack()
    {
        if (damageOvertimeCo != null)
            StopCoroutine(damageOvertimeCo);

        currentEnemy = null;
        lastTimeAttacked = Time.time;
        busyWithAttack = false;
        CreateNewProjectile();
    }

    protected override bool CanAttack()
    {
        return base.CanAttack() && busyWithAttack == false;
    }

    // Won't let go until attack is over
    protected override void LoseTargetIfNeeded()
    {
        if (busyWithAttack == false)
            base.LoseTargetIfNeeded();
    }

    private void CreateNewProjectile()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, projectileTransform.position, projectileTransform.rotation, towerHead);
        currentProjectile = newProjectile.GetComponent<ProjectileHarpoon>();
    }

    public void ActivateAttack()
    {
        currentEnemy.SlowEnemy(slowEffect, overtimeEffectDuration);

        IDamagable damagable = currentEnemy.GetComponent<IDamagable>();
        damagable?.TakeDamage(initialDamage);

        damageOvertimeCo = StartCoroutine(DamageOvertimeCo(damagable));
    }

    private IEnumerator DamageOvertimeCo(IDamagable damagable)
    {
        float time = 0;

        // How often the damage is tick
        float damageFrequency = overtimeEffectDuration / damageOvertime;
        float damagePertick = damageOvertime / (overtimeEffectDuration / damageFrequency);

        while (time < overtimeEffectDuration)
        {
            damagable?.TakeDamage(damagePertick);
            yield return new WaitForSeconds(damageFrequency);
            time += damageFrequency;
        }

        ResetAttack();
    }
}
