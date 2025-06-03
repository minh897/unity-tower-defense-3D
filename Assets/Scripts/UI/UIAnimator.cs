using System.Collections;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public void ChangePosition(Transform transform, Vector3 offset, float duration = .1f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        StartCoroutine(TransitPositionRoutine(rectTransform, offset, duration));
    }

    private IEnumerator TransitPositionRoutine(RectTransform rectTransform, Vector3 offset, float duration)
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
}
