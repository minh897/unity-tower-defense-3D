using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private float showCaseScale = 1.1f;
    [SerializeField] private float scaleUpDuration = .25f;

    private UIAnimator uIAnim;
    private RectTransform myRect;
    private Coroutine scaleRoutine;

    void Awake()
    {
        uIAnim = GetComponent<UIAnimator>();
        myRect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Stop another coroutine from starting
        // Avoid same coroutine stacking on top of each other
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);
            
        scaleRoutine = StartCoroutine(uIAnim.ChangeScaleRoutine(myRect, showCaseScale, scaleUpDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);
        
        scaleRoutine = StartCoroutine(uIAnim.ChangeScaleRoutine(myRect, 1, scaleUpDuration));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        myRect.localScale = new(1, 1, 1);
    }
}
