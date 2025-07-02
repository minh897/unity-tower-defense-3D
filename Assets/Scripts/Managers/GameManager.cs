using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int currency;

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
        currentHP = maxHP;

        if (IsTestingLevel())
        {
            currency += 9999;
            currentHP += 9999;
        }

        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
        uiInGame.UpdateCurrencyText(currency);
    }

    public void CompleteLevel()
    {
        StartCoroutine(CompleteLevelCo());
    }

    public void PrepareLevel(int levelCurrency, WaveManager newWaveManager)
    {
        isGameLost = false;
        enemiesKilled = 0;
        currency = levelCurrency;
        currentHP = maxHP;
        currentActiveWaveManager = newWaveManager;

        uiInGame.UpdateCurrencyText(currentHP);
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);

        newWaveManager.ActivateWaveManager();
    }

    public void UpdateHP(int changeValue)
    {
        currentHP += changeValue;
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
        uiInGame.ShakeHealthUI();

        if (currentHP <= 0 && isGameLost == false)
            StartCoroutine(FailLevel());
    }

    public void UpdateCurrency(int changeValue)
    {
        enemiesKilled++;
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

        yield return cameraEffects.GetActiveCameraCo();

        if (levelManager.HasNoMoreLevels())
            uiInGame.EnableVictoryUI(true);
        else
        {
            uiInGame.EnableLevelCompletionUI(true);
            PlayerPrefs.SetInt(levelManager.GetNextLevelName() + "unlocked", 1);
        }
    }

    public bool IsTestingLevel() => levelManager == null;

}
