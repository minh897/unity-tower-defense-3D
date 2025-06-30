using System.Collections;
using UnityEngine;

public class TowerSpider : Tower
{
    [Header("Tower Spider Details")]
    [SerializeField] private float damage;
    [SerializeField] private float attackTimeMultiplier = .4f; // The percentage of the time used for attacking (40%)
    [SerializeField] private float reloadTimeMultiplier = .6f; // The percentage of the time used for reloading (60%)
    [SerializeField] private GameObject spiderPrefab;
    [Space]

    [SerializeField] private Transform[] webSet;
    [SerializeField] private Transform[] attachPointSet;
    [SerializeField] private Transform[] attachPointRefSet;

    private int spiderIndex;
    private Vector3 spiderPointOffset = new(0, -.18f, 0);
    private GameObject[] activeSpiders;

    protected override void Start()
    {
        base.Start();
        InitializeSpiders();
        reloadTimeMultiplier = 1 - attackTimeMultiplier;
    }

    protected override void Update()
    {
        base.Update();
        UpdateAttachPointsPosition();
    }

    // For testing without enemy
    protected override bool CanAttack()
    {
        return Time.time > lastTimeAttacked + attackCoolDown;
    }

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(AttackCo());
    }

    private void UpdateAttachPointsPosition()
    {
        for (int i = 0; i < attachPointSet.Length; i++)
        {
            attachPointSet[i].position = attachPointRefSet[i].position;
        }
    }
    
    private void InitializeSpiders()
    {
        activeSpiders = new GameObject[attachPointSet.Length];

        for (int i = 0; i < activeSpiders.Length; i++)
        {
            GameObject newSpider = objectPool.Get(spiderPrefab, attachPointSet[i].position + spiderPointOffset, Quaternion.identity, attachPointSet[i]);
            activeSpiders[i] = newSpider;
        }
    }

    private IEnumerator AttackCo()
    {
        Transform currentWeb = webSet[spiderIndex];
        Transform currentAttachPoint = attachPointSet[spiderIndex];

        // The cooldown is split into 4 parts (because we have 4 spider drones)
        // only 1/4th of it is used for attacking and reloading for each spider
        // So if attackCoolDown is 4, then each spider with take up one second
        float attackTime = (attackCoolDown / 4) * attackTimeMultiplier;
        float reloadTime = (attackCoolDown / 4) * reloadTimeMultiplier;

        // Attacking phase
        yield return ChangeScaleCo(currentWeb, 1, attackTime);
        activeSpiders[spiderIndex].GetComponent<ProjectileSpider>().SetupSpider(damage);

        // Reloading phase
        yield return ChangeScaleCo(currentWeb, .1f, reloadTime);
        activeSpiders[spiderIndex] = objectPool.Get(spiderPrefab, currentAttachPoint.position + spiderPointOffset, Quaternion.identity, currentAttachPoint);

        // Wraps around to 0 if it reaches the end
        spiderIndex = (spiderIndex + 1) % attachPointSet.Length;
    }

    public IEnumerator ChangeScaleCo(Transform obj, float newScale, float duration = .25f)
    {
        float time = 0;

        Vector3 initialScale = obj.localScale;
        Vector3 targetScale = new(1, newScale, 1);

        while (time < duration)
        {
            obj.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        obj.localScale = targetScale;
    }

}
