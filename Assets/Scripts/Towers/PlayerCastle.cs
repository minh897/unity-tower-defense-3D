using UnityEngine;

public class PlayerCastle : MonoBehaviour
{
    private GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().RemoveEnemy();

            if (gameManager == null)
                gameManager = FindFirstObjectByType<GameManager>();

            if (gameManager != null)
                gameManager.UpdateHP(-1);
        }
    }
}
