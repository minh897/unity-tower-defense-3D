using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    [SerializeField] private int currency;

    private UIInGame uIInGame;
    private WaveManager currentActiveWaveManager;

    void Awake()
    {
        uIInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
    }

    void Start()
    {
        currentHP = maxHP;
        uIInGame.UpdateHealthPointUIText(currentHP, maxHP);
    }

    public void UpdateGameManager(int levelCurrency, WaveManager newWaveManager)
    {
        currentActiveWaveManager = newWaveManager;
        currency = levelCurrency;
        currentHP = maxHP;

        uIInGame.UpdateCurrencyText(levelCurrency);
        uIInGame.UpdateHealthPointUIText(currentHP, maxHP);
    }

    public void UpdateHP(int changeValue)
    {
        currentHP += changeValue;
        uIInGame.UpdateHealthPointUIText(currentHP, maxHP);
        uIInGame.ShakeHealthUI();
    }

    public void UpdateCurrency(int changeValue)
    {
        currency += changeValue;
        uIInGame.UpdateCurrencyText(currency);
    }

    public bool HasEnoughCurrency(int price)
    {
        if (price < currency)
        {
            currency -= price;
            uIInGame.UpdateCurrencyText(currency);
            return true;
        }

        return false;
    }
}
