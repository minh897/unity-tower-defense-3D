using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int currency;

    private UIInGame uiInGame;
    private WaveManager currentActiveWaveManager;
    private LevelManager levelManager;
    private bool isGameLost;

    void Awake()
    {
        uiInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    void Start()
    {
        currentHP = maxHP;
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
    }

    public void FailLevel()
    {
        // Only trigger once
        // Prevent keep failing when multiple enemy enter the castle
        if (isGameLost)
            return;

        isGameLost = true;
        currentActiveWaveManager.DeactivateWaveManager();
        uiInGame.EnableDefeatUI(true);
    }

    public void CompleteLevel()
    {
        string currentLevelName = levelManager.currentLevelName;

        // Get a scene (level) index by that scene name. Plus 1 to get the next level
        int nextLevelIndex = SceneUtility.GetBuildIndexByScenePath(currentLevelName) + 1;

        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            uiInGame.EnableVictoryUI(true);
            Debug.LogWarning("You beat the game");
        }
        else
            levelManager.LoadLevel("Level_" + nextLevelIndex);
    }

    public void UpdateGameManager(int levelCurrency, WaveManager newWaveManager)
    {
        isGameLost = false;
        currency = levelCurrency;
        currentHP = maxHP;
        currentActiveWaveManager = newWaveManager;

        uiInGame.UpdateCurrencyText(currentHP);
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
    }

    public void UpdateHP(int changeValue)
    {
        currentHP += changeValue;
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
        uiInGame.ShakeHealthUI();

        if (currentHP <= 0)
            FailLevel();
    }

    public void UpdateCurrency(int changeValue)
    {
        currency += changeValue;
        uiInGame.UpdateCurrencyText(currency);
    }

    public bool HasEnoughCurrency(int price)
    {
        if (price < currency)
        {
            currency -= price;
            uiInGame.UpdateCurrencyText(currency);
            return true;
        }

        return false;
    }
}
