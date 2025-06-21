using System.Collections;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [Header("Impact Details")]
    [SerializeField] private float defaultShieldGlow = 1;
    [SerializeField] private float impactShieldGlow = 3;
    [SerializeField] private float impactSpeed;
    [SerializeField] private float impactResetDur = .1f;
    [SerializeField] private float impactScaleMulti = .97f;
    [SerializeField] private Material shieldMat;

    private float defaultScale;
    private string fresnelPara = "_FresnelPower";
    private Coroutine currentCo;

    void Start()
    {
        defaultScale = transform.localScale.x;
    }

    public void ActivateShieldImpact()
    {
        if (currentCo != null)
            StopCoroutine(currentCo);

        currentCo = StartCoroutine(ImpactCo());
    }

    private IEnumerator ImpactCo()
    {
        yield return StartCoroutine(ShieldChangeCo(impactShieldGlow, impactSpeed, defaultScale * impactScaleMulti));
        StartCoroutine(ShieldChangeCo(defaultShieldGlow, impactResetDur, defaultScale));
    }

    private IEnumerator ShieldChangeCo(float targetGlow, float glowDuration, float targetScale)
    {
        float time = 0;
        float startGlow = shieldMat.GetFloat(fresnelPara);
        Vector3 initialScale = transform.localScale;
        Vector3 newScale = new(targetScale, targetScale, targetScale);

        while (time < glowDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, newScale, time / glowDuration);

            float newGlow = Mathf.Lerp(startGlow, targetGlow, time / glowDuration);
            shieldMat.SetFloat(fresnelPara, newGlow);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = newScale;
        shieldMat.SetFloat(fresnelPara, targetGlow);
    }
}
