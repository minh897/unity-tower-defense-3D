using UnityEngine;

public class UIBuildButton : MonoBehaviour
{
    [SerializeField] private float yPosOffset;

    private bool isActive;
    private UIAnimator uIAnimator;

    void Awake()
    {
        uIAnimator = GetComponentInParent<UIAnimator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ShowBuildButtons();
    }

    public void ShowBuildButtons()
    {
        isActive = !isActive;
        float changeYOffset = isActive ? yPosOffset : -yPosOffset;
        Vector3 changeOffset = new(0, changeYOffset);

        uIAnimator.ChangePosition(transform, changeOffset);
    }
}
