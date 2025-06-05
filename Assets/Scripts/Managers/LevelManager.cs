using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<TowerUnlockData> towerUnlocks;

    void Start()
    {
        UnlockAvailableTowers();
    }

    private void UnlockAvailableTowers()
    {
        UI ui = FindFirstObjectByType<UI>();

        foreach (var unlockData in towerUnlocks)
        {
            foreach (var buildButton in ui.uiBuildButton.GetBuildButtons())
            {
                buildButton.UnlockTower(unlockData.towerName, unlockData.isUnlocked);
            }
        }

        ui.uiBuildButton.UpdateUnlockedButton();
    }

    [ContextMenu("InitializeTowerData")]
    private void InitializeTowerData()
    {
        towerUnlocks.Clear();

        towerUnlocks.Add(new TowerUnlockData("Crossbow", false));
        towerUnlocks.Add(new TowerUnlockData("Cannon", false));
        towerUnlocks.Add(new TowerUnlockData("Minigun", false));
        towerUnlocks.Add(new TowerUnlockData("Spider Nest", false));
        towerUnlocks.Add(new TowerUnlockData("Harpoon", false));
        towerUnlocks.Add(new TowerUnlockData("Hammer", false));
        towerUnlocks.Add(new TowerUnlockData("Just Fan", false));
    }
}

[System.Serializable]
public class TowerUnlockData
{
    public string towerName;
    public bool isUnlocked;

    public TowerUnlockData(string towerNamePara, bool isUnlockedPara)
    {
        towerName = towerNamePara;
        isUnlocked = isUnlockedPara;
    }
}