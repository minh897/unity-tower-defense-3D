using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private Vector3 inMenuPosition;
    [SerializeField] private Quaternion inMenuRotation;
    [SerializeField] private Vector3 inGamePosition;
    [SerializeField] private Quaternion inGameRotation;

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchToGameView();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchToMenuView();
    }

    private void SwitchToMenuView()
    {
        StartCoroutine(ChangePositionAndRotation(inMenuPosition, inMenuRotation));
        cameraController.AdjustPicthValue(inMenuRotation.eulerAngles.x);
    }

    private void SwitchToGameView()
    {
        StartCoroutine(ChangePositionAndRotation(inGamePosition, inGameRotation));
        cameraController.AdjustPicthValue(inGameRotation.eulerAngles.x);
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
}
