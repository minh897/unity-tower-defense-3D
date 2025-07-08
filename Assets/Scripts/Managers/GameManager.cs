using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalCurrency;
    public static GameManager instance;

    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    private bool isGameLost;
    private LevelManager levelManager;
    private CameraEffects cameraEffects;

    public int enemiesKilled { get; private set; }
    public UIInGame uiInGame { get; private set; }

    public WaveManager currentActiveWaveManager;

    void Awake()
    {
        instance = this;

        uiInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
        levelManager = FindFirstObjectByType<LevelManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
    }

    void Start()
    {
        if (IsTestingLevel())
        {
            totalCurrency += 9999;
            maxHP += 9999;
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

        uiInGame.UpdateCurrencyText(totalCurrency);
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);

        currentActiveWaveManager.ActivateWaveManager();
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
        currentActiveWaveManager.DeactivateWaveManager();
        cameraEffects.FocusOnCastle();

        yield return cameraEffects.GetActiveCameraCo();

        uiInGame.EnableGameOverUI(true);
    }

    public IEnumerator CompleteLevelCo()
    {
        cameraEffects.FocusOnCastle();
        currentActiveWaveManager.DeactivateWaveManager();

        yield return cameraEffects.GetActiveCameraCo();

        uiInGame.EnableVictoryUI(true);
    }

    public bool IsTestingLevel() => levelManager == null;
}
