using UnityEngine;

public class UIBuildButton : MonoBehaviour
{
    [SerializeField] private float yPosOffset;
    [SerializeField] private float openAnimDuration = .1f;

    private bool isBuildMenuActive;
    private UIAnimator uIAnimator;
    private UIBuildButtonHoverEffect[] buildButtons;

    void Awake()
    {
        uIAnimator = GetComponentInParent<UIAnimator>();
        buildButtons = GetComponentsInChildren<UIBuildButtonHoverEffect>();
    }

    public void ShowBuildButtons(bool enable)
    {
        isBuildMenuActive = enable;

        float changeYOffset = isBuildMenuActive ? yPosOffset : -yPosOffset;
        float methodDelay = isBuildMenuActive ? openAnimDuration : 0;

        uIAnimator.ChangePosition(transform, new(0, changeYOffset), openAnimDuration);
        Invoke(nameof(ToggleButtonMovement), methodDelay);
    }

    private void ToggleButtonMovement()
    {
        foreach (var button in buildButtons)
        {
            button.ToggleMovement(isBuildMenuActive);
        }
    }
}
