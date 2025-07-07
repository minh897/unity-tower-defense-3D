using System.Collections.Generic;
using UnityEngine;

public class UIBuildButtonsHolder : MonoBehaviour
{
    [SerializeField] private float yPosOffset;
    [SerializeField] private float openAnimDuration = .1f;

    private bool isBuildMenuActive;
    private UIAnimator uIAnimator;
    private UIBuildButtonHoverEffect[] buildButtonEffects;
    private UIBuildButton[] buildButtons;

    private List<UIBuildButton> unlockedButtons;
    private UIBuildButton lastSelectedButton;
    private Transform towerPreview;

    void Awake()
    {
        uIAnimator = GetComponentInParent<UIAnimator>();
        buildButtonEffects = GetComponentsInChildren<UIBuildButtonHoverEffect>();
        buildButtons = GetComponentsInChildren<UIBuildButton>();
    }

    void Update()
    {
        CheckBuildButtonHotkey();
    }

    private void CheckBuildButtonHotkey()
    {
        if (isBuildMenuActive == false)
            return;

        for (int i = 0; i < unlockedButtons.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectNewButton(i);
                    break;
                }
            }

        if (lastSelectedButton != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                lastSelectedButton.ConfirmTowerBuilding();
                towerPreview = null;
            }

            if (Input.GetKeyDown(KeyCode.Q))
                RotateTarget(towerPreview, -90);

            if (Input.GetKeyDown(KeyCode.E))
                RotateTarget(towerPreview, 90);
        }
    }

    private void RotateTarget(Transform target, float angle)
    {
        if (target == null)
            return;

        target.Rotate(0, angle, 0);
        target.GetComponent<ForwardAttackDisplay>().UpdateLines();
    }

    public void SelectNewButton(int buttonIndex)
    {
        if (buttonIndex >= unlockedButtons.Count)
            return;

        foreach (var button in unlockedButtons)
        {
            button.TogglePreviewVisual(false);
        }

        UIBuildButton selectedButton = unlockedButtons[buttonIndex];
        selectedButton.TogglePreviewVisual(true);
    }

    public void UpdateUnlockedButton()
    {
        unlockedButtons = new List<UIBuildButton>();

        foreach (var button in buildButtons)
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

        uIAnimator.StartChangePosition(transform, new(0, changeYOffset), openAnimDuration);
        Invoke(nameof(ToggleButtonMovement), methodDelay);
    }

    private void ToggleButtonMovement()
    {
        foreach (var button in buildButtonEffects)
        {
            button.ToggleMovement(isBuildMenuActive);
        }
    }

    public void SetLastSelected(UIBuildButton newLastSelected, Transform newPreview)
    {
        lastSelectedButton = newLastSelected;
        towerPreview = newPreview;
    }

    public UIBuildButton[] GetBuildButtons() => buildButtons;

    public List<UIBuildButton> GetUnlockedButtons() => unlockedButtons;

    public UIBuildButton GetLastSelectedButton() => lastSelectedButton;
}
