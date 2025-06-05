using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiBuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    private TowerAttackRangeDisplay towerAttackRangeDisplay;

    void Awake()
    {
        ui = GetComponentInParent<UI>();
        buildManager = FindFirstObjectByType<BuildManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        gameManager = FindFirstObjectByType<GameManager>();
        towerAttackRangeDisplay = FindFirstObjectByType<TowerAttackRangeDisplay>(FindObjectsInactive.Include);

        if (towerToBuild != null)
            towerAttackRange = towerToBuild.GetComponent<Tower>().GetAttackRange();
    }

    void OnValidate()
    {
        towerNameText.text = towerName;
        towerPriceText.text = towerPrice + "";
        gameObject.name = "Build Button - " + towerName;    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildSlot slotToUse = buildManager.GetSelectedBuildSlot();
        towerAttackRangeDisplay.ShowAttackRange(true, towerAttackRange, slotToUse.GetBuildPosition(.5f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        towerAttackRangeDisplay.ShowAttackRange(false, towerAttackRange, Vector3.zero);
    }

    public void UnlockTower(string towerNameToCheck, bool unlockStatus)
    {
        if (towerNameToCheck != towerName)
            return;

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

        BuildSlot slotToUse = buildManager.GetSelectedBuildSlot();
        buildManager.CancelBuildAction();

        slotToUse.SnapToDefaultPosition();
        slotToUse.SetSlotAvailable(false);

        cameraEffects.ShakeScreen(.15f, .02f);

        GameObject newTower = Instantiate(towerToBuild, slotToUse.GetBuildPosition(towerCenterY), Quaternion.identity);
    }

}
