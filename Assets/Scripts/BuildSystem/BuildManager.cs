using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private UI ui;
    private BuildSlot selectedBuildSlot;

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildAction();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Collect hit collider info by using Raycast from the camera to the clicked mouse position
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                // Return true or false whether BuildSlot component was found on the hit object
                bool isBuildSlotClicked = hit.collider.GetComponent<BuildSlot>() == null;

                if (isBuildSlotClicked)
                    CancelBuildAction();
            }
        }
    }

    public void CancelBuildAction()
    {
        if (selectedBuildSlot == null)
            return;
        
        selectedBuildSlot.UnSelectTile();
        selectedBuildSlot = null;
        DisableBuildMenu();
    }

    public void SelectBuildSlot(BuildSlot buildSlot)
    {
        if (selectedBuildSlot != null)
            selectedBuildSlot.UnSelectTile();

        selectedBuildSlot = buildSlot;
    }

    public void EnableBuildMenu()
    {
        if (selectedBuildSlot != null)
            return;

        ui.uiBuildButton.ShowBuildButtons(true);
    }

    private void DisableBuildMenu()
    {
        ui.uiBuildButton.ShowBuildButtons(false);
    }

    public BuildSlot GetSelectedBuildSlot() => selectedBuildSlot;
}
