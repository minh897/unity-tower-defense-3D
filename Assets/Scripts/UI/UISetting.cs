using TMPro;
using UnityEngine;
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

    CameraController cameraController;

    void Awake()
    {
        cameraController = FindFirstObjectByType<CameraController>();
    }

    public void AdjustKeyboardSense(float changeValue)
    {
        float newSense = Mathf.Lerp(minKeyboardSense, maxKeyboardSense, changeValue);
        cameraController.AdjustKeyboardMoveSpeed(newSense);
        keyboardSenseText.text = Mathf.RoundToInt(changeValue * 100) + "%";
    }

    public void AdjustMouseSense(float changeValue)
    {
        float newSense = Mathf.Lerp(minKeyboardSense, maxKeyboardSense, changeValue);
        cameraController.AdjustMouseMoveSpeed(newSense);
        mouseSenseText.text = Mathf.RoundToInt(changeValue * 100) + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSenseParameter, keyboardSenseSlider.value);
        PlayerPrefs.SetFloat(mouseSenseParameter, mouseSenseSlider.value);
    }

    void OnEnable()
    {
        keyboardSenseSlider.value = PlayerPrefs.GetFloat(keyboardSenseParameter, .5f);
        mouseSenseSlider.value = PlayerPrefs.GetFloat(mouseSenseParameter, .5f);
    }
}
