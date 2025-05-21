using System.Collections;
using UnityEngine;

public class CrossbowVisual : MonoBehaviour
{
    [Header("Tower Head Emission Visual")]
    [SerializeField] private float maxIntensity = 150f;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private LineRenderer[] lineRenderers;

    [Space]

    [Header("Front String Visual")]
    [SerializeField] private LineRenderer frontLine_L;
    [SerializeField] private LineRenderer frontLine_R;
    [SerializeField] private Transform frontStartPoint_L;
    [SerializeField] private Transform frontStartPoint_R;
    [SerializeField] private Transform frontEndPoint_L;
    [SerializeField] private Transform frontEndPoint_R;

    [Space]

    [Header("Back String Visual")]
    [SerializeField] private LineRenderer backLine_L;
    [SerializeField] private LineRenderer backLine_R;
    [SerializeField] private Transform backStartPoint_L;
    [SerializeField] private Transform backStartPoint_R;
    [SerializeField] private Transform backEndPoint_L;
    [SerializeField] private Transform backEndPoint_R;

    [Header("Rotor Visual")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorUnloadPoint;
    [SerializeField] private Transform rotorLoadPoint;

    [Space]

    [SerializeField] private float attackVisualDur = .1f;
    [SerializeField] private LineRenderer attackLine;

    private float currentIntensity;
    private TowerCrossbow mainTower;
    private Material materialInstance;

    void Awake()
    {
        mainTower = GetComponent<TowerCrossbow>();

        materialInstance = new Material(meshRenderer.material);
        meshRenderer.material = materialInstance;

        SetupMaterialLR();

        StartCoroutine(ChangeEmission(1));
    }

    void Update()
    {
        UpdateEmissionColor();

        UpdateStringVisual(frontLine_L, frontStartPoint_L, frontEndPoint_L);
        UpdateStringVisual(frontLine_R, frontStartPoint_R, frontEndPoint_R);
        UpdateStringVisual(backLine_L, backStartPoint_L, backEndPoint_L);
        UpdateStringVisual(backLine_R, backStartPoint_R, backEndPoint_R);
    }

    public void PlayAttackFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
    }

    public void PlayReloadFX(float duration)
    {
        float newDuration = duration / 2;
        StartCoroutine(ChangeEmission(newDuration));
        StartCoroutine(ChangeRotorPosition(newDuration)); 
    }

    private void SetupMaterialLR()
    {
        foreach (var line in lineRenderers)
        {
            line.material = materialInstance;
        }
    }

    private void UpdateStringVisual(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);

        // Convert the current emission color to gamma space
        // Make sure the color is not too dark or too bright
        emissionColor *= Mathf.LinearToGammaSpace(currentIntensity);

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

    private IEnumerator ChangeRotorPosition(float duration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloadPoint.position, rotorLoadPoint.position, tValue);
            yield return null;
        }

        rotor.position = rotorLoadPoint.position;
    }
}
