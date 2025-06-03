using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    public void ChangePosition(Transform transform, Vector3 offset, float duration = .1f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        StartCoroutine(ChangePositionRoutine(rectTransform, offset, duration));
    }

    public void ChangeScale(Transform transform, float targetScale, float duration = .25f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        StartCoroutine(ChangeScaleRoutine(rectTransform, targetScale, duration));
    }

    public void ChangeColor(Image image, float targetAlpha, float duration)
    {
        StartCoroutine(ChangeColorRoutine(image, targetAlpha, duration));
    }

    private IEnumerator ChangePositionRoutine(RectTransform rectTransform, Vector3 offset, float duration)
    {
        float time = 0;

        Vector3 initialPos = rectTransform.anchoredPosition;
        Vector3 targetPos = initialPos + offset;

        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(initialPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }

    public IEnumerator ChangeScaleRoutine(RectTransform rectTransform, float newScale, float duration = .25f)
    {
        float time = 0;
        Vector3 initialScale = rectTransform.localScale;
        Vector3 targetScale = new(newScale, newScale, newScale);

        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }

    private IEnumerator ChangeColorRoutine(Image image, float targetAlpha, float duration)
    {
        float time = 0;
        Color currentcolor = image.color;
        float startAlpha = currentcolor.a;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = new(currentcolor.r, currentcolor.g, currentcolor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        image.color = new(currentcolor.r, currentcolor.g, currentcolor.b, targetAlpha);
    }
}
