using TMPro;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [Space]

    [SerializeField] private float waveTimerOffset;
    [SerializeField] private UITextBlinkEffect waveTimerTextBlinkEffect;
    [SerializeField] private Transform waveTimerTrans;
    [SerializeField] private Coroutine waveTimerMoveRoutine;

    [Header("Victory & Defeat")]
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject defeatUI;
    [SerializeField] private GameObject levelCompletedUI;

    private UI ui;
    private UIAnimator uIAnimator;
    private UIPauseMenu uiPauseMenu;
    private Vector3 waveTimerDefaultPos; 

    void Awake()
    {
        ui = GetComponentInParent<UI>();
        uIAnimator = GetComponentInParent<UIAnimator>();
        uiPauseMenu = ui.GetComponentInChildren<UIPauseMenu>(true);

        if (waveTimerTrans != null)
            waveTimerDefaultPos = waveTimerTrans.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ui.SwitchUIElement(uiPauseMenu.gameObject);
    }

    public void EnableVictoryUI(bool isEnable)
    {
        if (victoryUI != null)
            victoryUI.SetActive(isEnable);
    }

    public void EnableGameOverUI(bool isEnable)
    {
        if (defeatUI != null)
            defeatUI.SetActive(isEnable);
    }

    public void EnableLevelCompletionUI(bool isEnable)
    {
        if (levelCompletedUI != null)
            levelCompletedUI.SetActive(isEnable);
    }

    public void UpdateHealthPointUIText(int changeValue, int maxValue)
    {
        int newValue = maxValue - changeValue;
        healthPointsText.text = "Threat : " + newValue + "/" + maxValue;
    }

    public void ToggleWaveTimerUI(bool enable)
    {
        RectTransform waveTimerTransform = waveTimerTrans.GetComponent<RectTransform>();
        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;
        Vector3 offset = new(0, yOffset, 0);

        waveTimerMoveRoutine = StartCoroutine(uIAnimator.ChangePositionCo(waveTimerTransform, offset));
        waveTimerTextBlinkEffect.ToggleBlinkEffect(true);
    }

    public void DefaultWaveTimerPos()
    {
        if (waveTimerTrans == null)
            return;

        if (waveTimerMoveRoutine != null)
            StopCoroutine(waveTimerMoveRoutine);

        waveTimerTrans.localPosition = waveTimerDefaultPos;
    }

    public void ActivateNextWave()
    {
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        waveManager.StartNewWave();
    }

    public void UpdateCurrencyText(int changeValue) => currencyText.text = "Currency : " + changeValue;

    public void ShakeCurrencyUI() => ui.uiAnimator.StartShake(currencyText.transform.parent);

    public void ShakeHealthUI() => ui.uiAnimator.StartShake(healthPointsText.transform.parent);

}
