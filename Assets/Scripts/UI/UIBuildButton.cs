using UnityEngine;

public class UiBuildButton : MonoBehaviour
{
    [SerializeField] private int price = 50;
    [SerializeField] private float towerCenterY = .5f;
    [SerializeField] private GameObject towerToBuild;

    private BuildManager buildManager;
    private CameraEffects cameraEffects;
    private GameManager gameManager;

    void Awake()
    {
        buildManager = FindFirstObjectByType<BuildManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void BuildTower()
    {
        if (gameManager.HasEnoughCurrency(price) == false)
            return;

        if (towerToBuild == null)
            {
                Debug.LogWarning("Didn't tower assigned to this button");
                return;
            }

        BuildSlot slotToUse = buildManager.GetSelectedBuildSlot();
        buildManager.CancelBuildAction();
        slotToUse.SnapToDefaultPosition();
        cameraEffects.ShakeScreen(.15f, .02f);

        GameObject newTower = Instantiate(towerToBuild, slotToUse.GetBuildPosition(towerCenterY), Quaternion.identity);
    }
}
