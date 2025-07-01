using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TowerDrone : Tower
{
    [Header("Tower Drone Details")]
    [SerializeField] private float damage;
    [SerializeField] private float attackTimeMultiplier = .4f; // The percentage of the time used for attacking (40%)
    [SerializeField] private float reloadTimeMultiplier = .6f; // The percentage of the time used for reloading (60%)
    [SerializeField] private GameObject dronePrefab;
    [Space]

    [SerializeField] private Transform[] webSet;
    [SerializeField] private Transform[] attachPointSet;
    [SerializeField] private Transform[] attachPointRefSet;

    private int droneIndex;
    private Vector3 dronePointOffset = new(0, -.18f, 0);
    private GameObject[] activeDrones;

    protected override void Start()
    {
        base.Start();
        InitializeSpiders();
        reloadTimeMultiplier = 1 - attackTimeMultiplier;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateAttachPointsPosition();
    }

    // For testing without enemy
    // protected override bool CanAttack()
    // {
    //     return Time.time > lastTimeAttacked + attackCoolDown;
    // }

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
        activeDrones = new GameObject[attachPointSet.Length];

        for (int i = 0; i < activeDrones.Length; i++)
        {
            GameObject newDrone = objectPool.Get(dronePrefab, attachPointSet[i].position + dronePointOffset, Quaternion.identity, attachPointSet[i]);
            newDrone.SetActive(true);
            activeDrones[i] = newDrone;
        }
    }

    private IEnumerator AttackCo()
    {
        Transform currentWeb = webSet[droneIndex];
        Transform currentAttachPoint = attachPointSet[droneIndex];

        // The cooldown is split into 4 parts (for 4 drones)
        // Each drone only use 1/4th of the time for attacking and reloading
        // So if attackCoolDown is 4, then each drone with take up one second
        float attackTime = (attackCoolDown / 4) * attackTimeMultiplier;
        float reloadTime = (attackCoolDown / 4) * reloadTimeMultiplier;

        // Attacking phase
        yield return ChangeScaleCo(currentWeb, 1, attackTime);
        activeDrones[droneIndex].GetComponent<ProjectileDrone>().SetupDrone(damage);

        // Reloading phase
        yield return ChangeScaleCo(currentWeb, .1f, reloadTime);
        activeDrones[droneIndex] = objectPool.Get(dronePrefab, currentAttachPoint.position + dronePointOffset, Quaternion.identity, currentAttachPoint);
        activeDrones[droneIndex].SetActive(true);

        // Wraps around to 0 if it reaches the end
        droneIndex = (droneIndex + 1) % attachPointSet.Length;
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
