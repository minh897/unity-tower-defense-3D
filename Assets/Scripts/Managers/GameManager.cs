using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalCurrency;
    public static GameManager instance;

    public int enemiesKilled { get; private set; }
    public UIInGame uiInGame { get; private set; }
    public WaveManager activeWaveManager { get; private set; }

    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    private bool isGameLost;
    private int enemyAmount;
    private LevelManager levelManager;
    private CameraEffects cameraEffects;


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
            PrepareLevel(activeWaveManager, totalCurrency);
        }
    }

    public void CompleteLevel()
    {
        StartCoroutine(CompleteLevelCo());
    }

    public void PrepareLevel(WaveManager sceneWaveManager, int? sceneCurrency = 0)
    {
        isGameLost = false;
        enemiesKilled = 0;
        currentHP = maxHP;
        activeWaveManager = sceneWaveManager;

        if (sceneCurrency != 0)
            totalCurrency = (int)sceneCurrency;

        uiInGame.UpdateCurrencyText(totalCurrency);
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
        uiInGame.UpdateEnemyCountText(enemyAmount);

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
        enemiesKilled++;
        DecreaseEnemyCount();

        totalCurrency += changeValue;
        uiInGame.UpdateCurrencyText(totalCurrency);
    }

    public void DecreaseEnemyCount()
    {
        int remainingEnemies = activeWaveManager.GetEnemyCount() - enemiesKilled;
        uiInGame.UpdateEnemyCountText(remainingEnemies);

        if (remainingEnemies <= 0)
            activeWaveManager.HandleWaveCompletion();
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

    public bool IsTestingLevel() => levelManager == null;
}
