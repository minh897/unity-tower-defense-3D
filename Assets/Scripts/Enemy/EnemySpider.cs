using UnityEngine;

public class EnemySpider : Enemy
{
    [Header("EMP Attack Details")]
    [SerializeField] private float towerCheckRadius = 5;
    [SerializeField] private float empCooldown = 8;
    [SerializeField] private float empFXDuration = 3;
    [SerializeField] private GameObject empPrefab;
    [SerializeField] private LayerMask whatIsTower;
    private float empAttackTimer;

    protected override void Start()
    {
        base.Start();

        empAttackTimer = empCooldown;
    }

    protected override void Update()
    {
        base.Update();

        empAttackTimer -= Time.deltaTime;

        if (empAttackTimer < 0)
            AttemptToEMP();
    }

    private void AttemptToEMP()
    {
        Transform target = FindRandomTower();

        if (target == null)
            return;

        empAttackTimer = empCooldown;

        GameObject newEMP = Instantiate(empPrefab, transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
        newEMP.GetComponent<EnemySpiderEMP>().SetupEMP(empFXDuration, target.position);
    }

    private Transform FindRandomTower()
    {
        Collider[] towers = Physics.OverlapSphere(transform.position, towerCheckRadius, whatIsTower);

        if (towers.Length > 0)
            return towers[Random.Range(0, towers.Length)].transform.root;

        return null;
    }
}
