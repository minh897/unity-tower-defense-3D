using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    [Header("Level Details")]
    [SerializeField] private int levelCurrency = 1000;
    [SerializeField] private List<TowerUnlockData> towerUnlocks;

    [Header("Level Setup")]
    [SerializeField] private GridBuilder mainGrid;
    [SerializeField] private WaveManager myWaveManager;
    [SerializeField] private List<GameObject> objectsToDelete = new();

    private UI ui;
    private TileAnimator tileAnimator;
    private LevelManager levelManager;
    private GameManager gameManager;

    // Unity will automatically call this Start coroutine
    // to wait until it done with GetCurrentActiveCo
    // then activate WaveManager
    private IEnumerator Start()
    {
        UnlockAvailableTowers();

        if (CheckLevelLoadedToMainScene())
        {
            DeleteExtraObjects();

            levelManager.UpdateCurrentGrid(mainGrid);

            // Get TileAnimator in the Main Scene since we delete it above
            tileAnimator = FindFirstObjectByType<TileAnimator>();
            tileAnimator.ShowCurrentGrid(mainGrid, true);

            yield return tileAnimator.GetCurrentActiveRoutine();

            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.UpdateGameManager(levelCurrency, myWaveManager);

            ui = FindFirstObjectByType<UI>();
            ui.EnableInGameUI(true);

            myWaveManager.ActivateWaveManager();
        }
    }

    private bool CheckLevelLoadedToMainScene()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        return levelManager != null;
    }

    private void DeleteExtraObjects()
    {
        foreach (var obj in objectsToDelete)
        {
            Destroy(obj);
        }
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