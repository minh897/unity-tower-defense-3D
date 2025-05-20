using System.Collections;
using UnityEngine;

public class CrossbowVisual : MonoBehaviour
{
    [SerializeField] private LineRenderer attackLine;
    [SerializeField] private float attackVisualDur = .1f;

    [Header("Glowing Visual")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material materialInstance;
    [SerializeField] private float currentIntensity;
    [SerializeField] private float maxIntensity = 150f;

    private TowerCrossbow mainTower;

    void Awake()
    {
        mainTower = GetComponent<TowerCrossbow>();

        materialInstance = new Material(meshRenderer.material);
        meshRenderer.material = materialInstance;

        StartCoroutine(ChangeEmission(3));
    }

    public void PlayAttackFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
    }

    private IEnumerator FXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        mainTower.EnableRotation(false);
        attackLine.enabled = true;

        attackLine.SetPosition(0, startPoint);
        attackLine.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDur);

        mainTower.EnableRotation(true);
        attackLine.enabled = false;
    }

    private IEnumerator ChangeEmission(float duration)
    {
        // Record the moment when this coroutine start
        float startTime = Time.time;
        float startIntensity = 0f;

        // Repeat every frame until the full duration has passed
        while (Time.time - startTime < duration)
        {
            // Calculate the proportion of the duration has elapsed since this coroutine start
            float tValue = (Time.time - startTime) / duration;
            // Smoothly increase the intensity based on current progress (tValue)
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null; // wait for the next frame
        }

        currentIntensity = maxIntensity;
    }
}
