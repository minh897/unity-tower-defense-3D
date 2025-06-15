using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private float showCaseScale = 1.1f;
    [SerializeField] private float scaleUpDuration = .25f;
    [SerializeField] private UITextBlinkEffect uITextBlinkEffect;

    private UIAnimator uIAnim;
    private RectTransform myRect;
    private Coroutine scaleRoutine;
    private UI ui;

    void Awake()
    {
        uIAnim = GetComponent<UIAnimator>();
        myRect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Stop another coroutine from starting
        // Avoid same coroutine stacking on top of each other
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        AudioManager.instance?.PlaySFX(ui.onHoverSFX);

        if (uITextBlinkEffect != null)
            uITextBlinkEffect.ToggleBlinkEffect(false);

        scaleRoutine = StartCoroutine(uIAnim.ChangeScaleCo(myRect, showCaseScale, scaleUpDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        if (uITextBlinkEffect != null)
            uITextBlinkEffect.ToggleBlinkEffect(true);

        scaleRoutine = StartCoroutine(uIAnim.ChangeScaleCo(myRect, 1, scaleUpDuration));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance?.PlaySFX(ui.onClickSFX);
        myRect.localScale = new(1, 1, 1);
    }
}
