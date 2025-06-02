using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [Header("Keyboard Pan Speed")]
    [SerializeField] private Slider keyboardPanSpeedSlider;
    [SerializeField] private float minKeyboardPanSpeed = 60f;
    [SerializeField] private float maxKeyboardPanSpeed = 240f;
    [SerializeField] private string keyboardSensParameter = "keyboardSens";

    CameraController cameraController;

    void Awake()
    {
        cameraController = FindFirstObjectByType<CameraController>();
    }

    public void AdjustKeyboardPanSpeed(float value)
    {
        float newSpeed = Mathf.Lerp(minKeyboardPanSpeed, maxKeyboardPanSpeed, value);
        cameraController.AdjustKeyboardMoveSpeed(newSpeed);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSensParameter, keyboardPanSpeedSlider.value);
    }

    void OnEnable()
    {
        keyboardPanSpeedSlider.value = PlayerPrefs.GetFloat(keyboardSensParameter, .5f);
    }
}
