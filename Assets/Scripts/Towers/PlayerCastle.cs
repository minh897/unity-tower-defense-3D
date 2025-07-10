using UnityEngine;

public class PlayerCastle : MonoBehaviour
{
    private GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.RemoveEnemy();

            if (gameManager == null)
                gameManager = FindFirstObjectByType<GameManager>();

            if (gameManager != null)
            {
                gameManager.UpdateHP(-1);
                gameManager.DecreaseEnemyCount();
            }
        }
    }
}
