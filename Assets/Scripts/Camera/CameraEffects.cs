using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Camera Transition")]
    [SerializeField] private Vector3 inMenuPosition;
    [SerializeField] private Quaternion inMenuRotation;
    [Space]
    [SerializeField] private Vector3 inGamePosition;
    [SerializeField] private Quaternion inGameRotation;
    [Space]
    [SerializeField] private Vector3 levelSelectPosition;
    [SerializeField] private Quaternion levelSelectRotation;

    [Header("Camera Shake")]
    [Range(0.01f, 0.5f)]
    [SerializeField] private float shakeMagnitude;
    [Range(0.1f, 3f)]
    [SerializeField] private float shakeDuration;

    private Coroutine cameraCo;
    private CameraController cameraController;

    void Awake()
    {
        cameraController = GetComponent<CameraController>();
    }

    void Start()
    {
        SwitchToMenuView();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            ShakeScreen(shakeDuration, shakeMagnitude);
    }

    public void SwitchToLevelSelectView()
    {
        if (cameraCo != null)
            StopCoroutine(cameraCo);

        cameraCo = StartCoroutine(ChangePositionAndRotation(levelSelectPosition, levelSelectRotation));
        cameraController.AdjustPicthValue(levelSelectRotation.eulerAngles.x);
    }

    public void SwitchToMenuView()
    {
        if (cameraCo != null)
            StopCoroutine(cameraCo);

        cameraCo = StartCoroutine(ChangePositionAndRotation(inMenuPosition, inMenuRotation));
        cameraController.AdjustPicthValue(inMenuRotation.eulerAngles.x);
    }

    public void SwitchToGameView()
    {
        if (cameraCo != null)
            StopCoroutine(cameraCo);

        cameraCo = StartCoroutine(ChangePositionAndRotation(inGamePosition, inGameRotation));
        cameraController.AdjustPicthValue(inGameRotation.eulerAngles.x);
    }

    public void ShakeScreen(float refDuration, float refMagnitude)
    {
        StartCoroutine(ScreenShakeFX(refDuration, refMagnitude));
    }

    private IEnumerator ChangePositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float duration = 3, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        cameraController.EnableCamControl(false);

        float time = 0;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        cameraController.EnableCamControl(true);
    }

    private IEnumerator ScreenShakeFX(float duration, float magnitude)
    {
        Vector3 originalPosition = cameraController.transform.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            cameraController.transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraController.transform.position = originalPosition;
    }
}
