using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string towerName;
    [SerializeField] private int towerPrice = 50;
    [Space]
    [SerializeField] private float towerCenterY = .5f;
    [SerializeField] private GameObject towerToBuild;

    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerPriceText;

    [SerializeField] private float towerAttackRange = 3;

    private UI ui;
    private BuildManager buildManager;
    private CameraEffects cameraEffects;
    private GameManager gameManager;
    private UIBuildButtonsHolder buildButtonHolder;
    private UIBuildButtonHoverEffect onHoverEffect;

    private TowerPreview towerPreview;

    public bool isUnlocked { get; private set; }

    void Awake()
    {
        onHoverEffect = GetComponent<UIBuildButtonHoverEffect>();

        ui = GetComponentInParent<UI>();
        buildButtonHolder = GetComponentInParent<UIBuildButtonsHolder>();

        buildManager = FindFirstObjectByType<BuildManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (towerToBuild != null)
            towerAttackRange = towerToBuild.GetComponent<Tower>().GetAttackRange();
    }

    void Start()
    {
        CreateTowerPreview();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buildManager.OnMouseOverUI(true);

        // Turn off the preview visual for other button in UIBuildButtonHolder
        foreach (var button in buildButtonHolder.GetBuildButtons())
        {
            if (button.gameObject.activeSelf)
                button.TogglePreviewVisual(false);
        }

        // Toggle the visual for the selected button
        TogglePreviewVisual(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buildManager.OnMouseOverUI(false);
    }

    // Toggle tower visual preview during tower placement
    public void TogglePreviewVisual(bool isSelect)
    {
        BuildSlot buildSlot = buildManager.GetSelectedBuildSlot();

        if (buildSlot == null)
            return;

        Vector3 previewPosition = buildSlot.GetBuildPosition(1);

        towerPreview.gameObject.SetActive(isSelect);
        towerPreview.ShowPreview(isSelect, previewPosition);
        onHoverEffect.ShowButton(isSelect);
        buildButtonHolder.SetLastSelected(this);
    }

    // Create a preview game object version of a tower
    private void CreateTowerPreview()
    {
        GameObject newPreview = Instantiate(towerToBuild, Vector3.zero, Quaternion.identity);

        // Add TowerPreview component to newPreview and store TowerPreview in towerPreview for future use
        towerPreview = newPreview.AddComponent<TowerPreview>();
        towerPreview.gameObject.SetActive(false);
    }

    public void UnlockTower(string towerNameToCheck, bool unlockStatus)
    {
        if (towerNameToCheck != towerName)
            return;

        isUnlocked = unlockStatus;
        gameObject.SetActive(unlockStatus);
    }

    public void BuildTower()
    {
        if (gameManager.HasEnoughCurrency(towerPrice) == false)
        {
            ui.uIInGame.ShakeCurrencyUI();
            return;
        }

        if (towerToBuild == null)
        {
            Debug.LogWarning("Didn't tower assigned to this button");
            return;
        }

        // Check if we have the current selected button
        if (ui.uiBuildButton.GetLastSelectedButton() == null)
            return;

        BuildSlot slotToUse = buildManager.GetSelectedBuildSlot();
        buildManager.CancelBuildAction();

        slotToUse.SnapToDefaultPosition();
        slotToUse.SetSlotAvailable(false);

        ui.uiBuildButton.SetLastSelected(null);

        cameraEffects.ShakeScreen(.15f, .02f);

        GameObject newTower = Instantiate(towerToBuild, slotToUse.GetBuildPosition(towerCenterY), Quaternion.identity);
    }

    private void OnValidate()
    {
        towerNameText.text = towerName;
        towerPriceText.text = towerPrice + "";
        gameObject.name = "Build Button - " + towerName;
    }

}
