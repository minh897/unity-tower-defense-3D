using TMPro;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private UITextBlinkEffect waveTimerTextBlinkEffect;
    [SerializeField] private float waveTimerOffset;

    private UI ui;
    private UIAnimator uIAnimator;
    private UIPauseMenu uiPauseMenu;

    void Awake()
    {
        ui = GetComponentInParent<UI>();
        uIAnimator = GetComponentInParent<UIAnimator>();
        uiPauseMenu = ui.GetComponentInChildren<UIPauseMenu>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
            ui.SwitchUIElemnt(uiPauseMenu.gameObject);
    }

    public void UpdateHealthPointUIText(int changeValue, int maxValue)
    {
        int newValue = maxValue - changeValue;
        healthPointsText.text = "Threat : " + newValue + "/" + maxValue;
    }

    public void ToggleWaveTimerUI(bool enable)
    {
        Transform waveTimerTransform = waveTimerText.transform.parent;
        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;
        Vector3 offset = new(0, yOffset, 0);

        uIAnimator.ChangePosition(waveTimerTransform, offset);
        waveTimerTextBlinkEffect.ToggleBlinkEffect(true);
    }

    public void ActivateNextWave()
    {
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        waveManager.StartNewWave();
    }

    public void UpdateCurrencyText(int changeValue) => currencyText.text = "Currency : " + changeValue;

    public void UpdateWaveTimerText(float changeValue) => waveTimerText.text = "Next Wave : " + changeValue.ToString("00") + " secs";

    public void ShakeCurrencyUI() => ui.uIAnimator.Shake(currencyText.transform.parent);

    public void ShakeHealthUI() => ui.uIAnimator.Shake(healthPointsText.transform.parent);

}
