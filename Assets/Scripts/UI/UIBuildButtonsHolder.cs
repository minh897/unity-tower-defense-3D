using System.Collections.Generic;
using UnityEngine;

public class UIBuildButtonsHolder : MonoBehaviour
{
    [SerializeField] private float yPosOffset;
    [SerializeField] private float openAnimDuration = .1f;

    private bool isBuildMenuActive;
    private UIAnimator uIAnimator;
    private UIBuildButtonHoverEffect[] buildButtonEffects;
    private UiBuildButton[] buildButtons;

    private List<UiBuildButton> unlockedButtons;

    void Awake()
    {
        uIAnimator = GetComponentInParent<UIAnimator>();
        buildButtonEffects = GetComponentsInChildren<UIBuildButtonHoverEffect>();
        buildButtons = GetComponentsInChildren<UiBuildButton>();
    }

    public void UpdateUnlockedButton()
    {
        unlockedButtons = new();

        foreach (var button in unlockedButtons)
        {
            if (button.isUnlocked)
                unlockedButtons.Add(button);
        }
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
        foreach (var button in buildButtonEffects)
        {
            button.ToggleMovement(isBuildMenuActive);
        }
    }

    public UiBuildButton[] GetBuildButtons() => buildButtons;

    public List<UiBuildButton> GetUnlockedButtons() => unlockedButtons;
}
