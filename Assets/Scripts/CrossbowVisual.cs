using System.Collections;
using UnityEngine;

public class CrossbowVisual : MonoBehaviour
{
    [SerializeField] private LineRenderer attackLine;
    [SerializeField] private float attackVisualDur = .1f;

    [Header("Glowing Visual")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float maxIntensity = 150f;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private TowerCrossbow mainTower;
    private Material materialInstance;
    private float currentIntensity;

    void Awake()
    {
        mainTower = GetComponent<TowerCrossbow>();

        materialInstance = new Material(meshRenderer.material);
        meshRenderer.material = materialInstance;

        StartCoroutine(ChangeEmission(1));
    }

    void Update()
    {
        UpdateEmissionColor();
    }

    public void PlayAttackFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
    }

    public void PlayReloadFX(float duration)
    {
        StartCoroutine(ChangeEmission(duration / 2));
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);

        // Convert the current emission color to gamma space
        // Make sure the color is not too dark or too bright
        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);

        materialInstance.SetColor("_EmissionColor", emissionColor);
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
