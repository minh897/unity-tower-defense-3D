using UnityEngine;

public class UiBuildButton : MonoBehaviour
{
    [SerializeField] private GameObject towerToBuild;
    [SerializeField] private float towerCenterY = .5f;

    private BuildManager buildManager;
    private CameraEffects cameraEffects;

    void Awake()
    {
        buildManager = FindFirstObjectByType<BuildManager>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
    }

    public void BuildTower()
    {
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
