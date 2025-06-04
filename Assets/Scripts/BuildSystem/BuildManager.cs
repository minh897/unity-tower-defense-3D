using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private BuildSlot selectedBuildSlot;

    public void SelectBuildSlot(BuildSlot buildSlot)
    {
        if (selectedBuildSlot != null)
            selectedBuildSlot.UnSelectTile();

        selectedBuildSlot = buildSlot;
    }
}
