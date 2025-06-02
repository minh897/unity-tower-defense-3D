using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    private UIInGame uIInGame;

    void Awake()
    {
        uIInGame = FindFirstObjectByType<UIInGame>();
    }

    void Start()
    {
        currentHP = maxHP;
        uIInGame.UpdateHealthPointUIText(currentHP, maxHP);
    }

    public void UpdateHP(int changeValue)
    {
        currentHP += changeValue;
        uIInGame.UpdateHealthPointUIText(currentHP, maxHP);
    }
}
