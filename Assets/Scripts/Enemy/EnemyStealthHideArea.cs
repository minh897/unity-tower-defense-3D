using UnityEngine;

public class EnemyStealthHideArea : MonoBehaviour
{
    private EnemyStealth enemyStealth;

    private void Awake()
    {
        enemyStealth = GetComponentInParent<EnemyStealth>();
    }

    void OnTriggerEnter(Collider other)
    {
        AddEnemyToHideList(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        AddEnemyToHideList(other, false);
    }

    private void AddEnemyToHideList(Collider enemyCollider, bool isEnemyAdd)
    {
        Enemy newEnemy = enemyCollider.GetComponent<Enemy>();

        if (newEnemy == null)
            return;

        if (newEnemy.GetEnemyType() == EnemyType.Stealth)
            return;

        if (isEnemyAdd)
            enemyStealth.GetEnemiesToHide().Add(newEnemy);
        else
            enemyStealth.GetEnemiesToHide().Remove(newEnemy);
    }
}
