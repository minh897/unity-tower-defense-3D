using UnityEngine;

public class ProjectileHarpoon : MonoBehaviour
{
    private bool isAttached;
    private float speed;
    private Enemy enemy;

    void Update()
    {
        if (enemy == null || isAttached)
            return;

        MoveTowardsEnemy();

        if (Vector3.Distance(transform.position, enemy.transform.position) < .4f)
            AttachToEnemy();
    }

    public void SetupProjectile(Enemy newEnemy, float newSpeed)
    {
        enemy = newEnemy;
        speed = newSpeed;
    }

    private void AttachToEnemy()
    {
        isAttached = true;
        transform.parent = enemy.transform;
        //tower active damage
    }

    private void MoveTowardsEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);
        transform.forward = enemy.transform.position - transform.position;
    }

}
