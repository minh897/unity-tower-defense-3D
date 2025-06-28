using UnityEngine;

public class TowerFanRevealArea : MonoBehaviour
{
    private TowerFan towerFan;

    void Awake()
    {
        towerFan = GetComponentInParent<TowerFan>();
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
            towerFan.AddEnemyToReveal(enemy);
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
            towerFan.RemoveEnemyToReveal(enemy);
    }
}
