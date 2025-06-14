using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int currency;

    private bool isGameLost;
    private UIInGame uiInGame;
    private WaveManager currentActiveWaveManager;
    private LevelManager levelManager;
    private CameraEffects cameraEffects;

    void Awake()
    {
        uiInGame = FindFirstObjectByType<UIInGame>(FindObjectsInactive.Include);
        levelManager = FindFirstObjectByType<LevelManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
    }

    void Start()
    {
        currentHP = maxHP;
        uiInGame.UpdateHealthPointUIText(currentHP, maxHP);
    }

    public void CompleteLevel()
    {
        StartCoroutine(CompleteLevelCo());
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

        if (currentHP <= 0 && isGameLost == false)
            StartCoroutine(FailLevel());
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
        PlayerPrefs.SetInt(levelManager.GetNextLevelName() + "unlocked", 1);
        cameraEffects.FocusOnCastle();

        yield return cameraEffects.GetActiveCameraCo();

        if (levelManager.HasNoMoreLevels())
            uiInGame.EnableVictoryUI(true);
        else
            uiInGame.EnableLevelCompletionUI(true);
    }

}
