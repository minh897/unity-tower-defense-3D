using TMPro;
using UnityEngine;

public class UiBuildButton : MonoBehaviour
{
    [SerializeField] private string towerName;
    [SerializeField] private int towerPrice = 50;
    [Space]
    [SerializeField] private float towerCenterY = .5f;
    [SerializeField] private GameObject towerToBuild;
    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerPriceText;

    private BuildManager buildManager;
    private CameraEffects cameraEffects;
    private GameManager gameManager;

    void Awake()
    {
        buildManager = FindFirstObjectByType<BuildManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnValidate()
    {
        towerNameText.text = towerName;
        towerPriceText.text = towerPrice + "";
        gameObject.name = "Build Button - " + towerName;    
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
            return;

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
