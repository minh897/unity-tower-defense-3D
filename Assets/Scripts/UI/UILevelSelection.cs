using UnityEngine;

public class UILevelSelection : MonoBehaviour
{
    private void MakeButtonClickable(bool canClick)
    {
        TileLevelButton[] levelButtons = FindObjectsByType<TileLevelButton>(FindObjectsSortMode.None);

        foreach (var button in levelButtons)
        {
            button.CheckIfLevelUnlocked();
            button.EnableClick(canClick);
        }
    }

    void OnEnable()
    {
        MakeButtonClickable(true);
    }

    void OnDisable()
    {
        MakeButtonClickable(false);
    }
}
