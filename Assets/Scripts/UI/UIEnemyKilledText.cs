using TMPro;
using UnityEngine;

public class UIEnemyKilledText : MonoBehaviour
{
    private TextMeshProUGUI myText;
    private GameManager gameManager;

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnEnable()
    {
        myText.text = "Enimies Killed: " + gameManager.enemiesKilled;
    }
}
