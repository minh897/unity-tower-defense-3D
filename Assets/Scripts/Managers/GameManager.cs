using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalCurrency;

    public UIInGame uiInGame { get; private set; }
    public WaveManager activeWaveManager { get; private set; }

    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    private bool isGameLost;
    private int enemiesKilled;
    private CameraEffects cameraEffects;
    private LevelManager levelManager;
    private EnemyPortal enemyPortal;

    void Awake()
    {
        instance = this;

        uiInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
        levelManager = FindFirstObjectByType<LevelManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        activeWaveManager = FindFirstObjectByType<WaveManager>();
    }

    void Start()
    {
        if (IsTestingLevel())
        {
            maxHP += 9999;
            totalCurrency += 9999;
            PrepareLevel();
        }
    }

    public void CompleteLevel()
    {
        StartCoroutine(CompleteLevelCo());
    }

    public void PrepareLevel()
    {
        isGameLost = false;
        enemiesKilled = 0;
        currentHP = maxHP;

        enemyPortal = activeWaveManager.enemyPortal;

        uiInGame.UpdateCurrencyText(totalCurrency);
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
        uiInGame.UpdateEnemyCountText(0);

        activeWaveManager.ActivateWaveManager();
    }

    public void UpdateHP(int changeValue)
    {
        currentHP += changeValue;
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
        uiInGame.ShakeHealthUI();

        if (currentHP <= 0 && isGameLost == false)
            StartCoroutine(FailLevel());
    }

    public void IncreaseCurrencyFromKill(int changeValue)
    {
        totalCurrency += changeValue;
        uiInGame.UpdateCurrencyText(totalCurrency);
    }

    public bool HasEnoughCurrency(int price)
    {
        if (price <= totalCurrency)
        {
            totalCurrency -= price;
            uiInGame.UpdateCurrencyText(totalCurrency);
            return true;
        }

        return false;
    }

    public IEnumerator FailLevel()
    {
        isGameLost = true;
        activeWaveManager.DeactivateWaveManager();
        cameraEffects.FocusOnCastle();

        yield return cameraEffects.GetActiveCameraCo();

        uiInGame.EnableGameOverUI(true);
    }

    public IEnumerator CompleteLevelCo()
    {
        cameraEffects.FocusOnCastle();
        activeWaveManager.DeactivateWaveManager();

        yield return cameraEffects.GetActiveCameraCo();

        uiInGame.EnableVictoryUI(true);
    }

    public bool IsTestingLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Test"))
            return true;

        return false;
    }

    public void IncreaseKilledEnemy() => enemiesKilled++;

    public int GetKilledEnemies() => enemiesKilled;
}
