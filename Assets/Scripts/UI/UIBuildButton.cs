using UnityEngine;

public class UiBuildButton : MonoBehaviour
{
    [SerializeField] private GameObject towerToBuild;
    [SerializeField] private float towerCenterY = .5f;

    private BuildManager buildManager;

    void Awake()
    {
        buildManager = FindFirstObjectByType<BuildManager>();
    }

    public void BuildTower()
    {
        if (towerToBuild == null)
            Debug.LogWarning("Didn't tower assigned to this button");

        BuildSlot slotToUse = buildManager.GetSelectedBuildSlot();
        buildManager.CancelBuildAction();

        GameObject newTower = Instantiate(towerToBuild, slotToUse.GetBuildPosition(towerCenterY), Quaternion.identity);
    }
}
