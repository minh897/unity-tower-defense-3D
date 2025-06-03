using TMPro;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private UITextBlinkEffect waveTimerTextBlinkEffect;
    [SerializeField] private float waveTimerOffset;

    private UIAnimator uIAnimator;

    void Awake()
    {
        uIAnimator = GetComponentInParent<UIAnimator>();
    }

    public void UpdateHealthPointUIText(int changeValue, int maxValue)
    {
        int newValue = maxValue - changeValue;
        healthPointsText.text = "Threat : " + newValue + "/" + maxValue;
    }

    public void UpdateCurrencyText(int changeValue) => currencyText.text = "Currency : " + changeValue;

    public void UpdateWaveTimerText(float changeValue) => waveTimerText.text = "Next Wave : " + changeValue.ToString("00") + " secs";

    public void ToggleWaveTimer(bool enable)
    {
        Transform waveTimerTransform = waveTimerText.transform.parent;
        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;
        Vector3 offset = new(0, yOffset, 0);

        uIAnimator.ChangePosition(waveTimerTransform, offset);
        waveTimerTextBlinkEffect.ToggleBlinkEffect(true);
        // waveTimerText.transform.parent.gameObject.SetActive(enable);
    }

    public void ActivateNextWave()
    {
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        waveManager.ForceNextWave();
    }
}
