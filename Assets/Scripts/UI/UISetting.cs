using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [Header("Keyboard Sensetivity")]
    [SerializeField] private float minKeyboardSense = 60f;
    [SerializeField] private float maxKeyboardSense = 240f;
    [SerializeField] private string keyboardSenseParameter = "keyboardSens";
    [SerializeField] private Slider keyboardSenseSlider;
    [SerializeField] private TextMeshProUGUI keyboardSenseText;

    [Header("Mouse Sensetivity")]
    [SerializeField] private float minMouseSense = 1f;
    [SerializeField] private float maxMouseSense = 10f;
    [SerializeField] private string mouseSenseParameter = "mouseSens";
    [SerializeField] private Slider mouseSenseSlider;
    [SerializeField] private TextMeshProUGUI mouseSenseText;

    [Space]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [Header("SFX Settings")]
    [SerializeField] private string sfxParameter;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderText;

    [Header("BGM Settings")]
    [SerializeField] private string bgmParameter;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmSliderText;


    private CameraController cameraController;

    void Awake()
    {
        cameraController = FindFirstObjectByType<CameraController>();
    }

    public void SFXSliderValue(float changeValue)
    {
        float newValue = MathF.Log10(changeValue) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
        sfxSliderText.text = Mathf.RoundToInt(changeValue * 100) + "%";
    }

    public void BGMSliderValue(float changeValue)
    {
        float newValue = MathF.Log10(changeValue) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
        bgmSliderText.text = Mathf.RoundToInt(changeValue * 100) + "%";
    }

    public void AdjustKeyboardSense(float changeValue)
    {
        float newSense = Mathf.Lerp(minKeyboardSense, maxKeyboardSense, changeValue);
        cameraController.AdjustKeyboardMoveSpeed(newSense);
        keyboardSenseText.text = Mathf.RoundToInt(changeValue * 100) + "%";
    }

    public void AdjustMouseSense(float changeValue)
    {
        float newSense = Mathf.Lerp(minMouseSense, maxMouseSense, changeValue);
        cameraController.AdjustMouseMoveSpeed(newSense);
        mouseSenseText.text = Mathf.RoundToInt(changeValue * 100) + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSenseParameter, keyboardSenseSlider.value);
        PlayerPrefs.SetFloat(mouseSenseParameter, mouseSenseSlider.value);
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }

    private void OnEnable()
    {
        keyboardSenseSlider.value = PlayerPrefs.GetFloat(keyboardSenseParameter, .6f);
        mouseSenseSlider.value = PlayerPrefs.GetFloat(mouseSenseParameter, .6f);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .6f);
    }
}
