using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private UI ui;
    private TileAnimator tileAnimator;
    private CameraEffects cameraEffects;
    private GridBuilder currentActiveGrid;

    public string currentLevelName { get; private set; }

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        currentActiveGrid = FindFirstObjectByType<GridBuilder>();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.L))
        //     LoadLevelFromMenu("Level_1");

        // if (Input.GetKeyDown(KeyCode.K))
        //     LoadMainMenu();

        // if (Input.GetKeyDown(KeyCode.R))
        //     RestartLevel();
    }

    private void RemoveAllEnemies()
    {
        Enemy[] enemiesArray = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemiesArray)
        {
            enemy.RemoveEnemy();
        }
    }

    private void RemoveAllTowers()
    {
        Tower[] towersArray = FindObjectsByType<Tower>(FindObjectsSortMode.None);

        foreach (Tower tower in towersArray)
        {
            Destroy(tower.gameObject);
        }
    }

    private void CleanUpScene()
    {
        RemoveAllEnemies();
        RemoveAllTowers();

        if (currentActiveGrid != null)
        {
            currentActiveGrid.ClearNavMeshData();
            tileAnimator.ShowCurrentGrid(currentActiveGrid, false);
        }
    }

    private IEnumerator LoadLevelFromMenuCo(string levelSceneName)
    {
        CleanUpScene();
        ui.EnableMainMenuUI(false);

        cameraEffects.SwitchToGameView();

        yield return tileAnimator.GetCurrentActiveRoutine();

        tileAnimator.EnableMainSceneObjects(false);

        LoadScene(levelSceneName);
    }

    private IEnumerator LoadMainMenuCo()
    {
        CleanUpScene();
        ui.EnableInGameUI(false);

        cameraEffects.SwitchToMenuView();

        // Delay until this GetCurrentActiveRoutine coroutine is finished
        yield return tileAnimator.GetCurrentActiveRoutine();

        UnloadCurrentScene();
        tileAnimator.EnableMainSceneObjects(true);
        tileAnimator.ShowMainGrid(true);

        yield return tileAnimator.GetCurrentActiveRoutine();

        ui.EnableMainMenuUI(true);
    }

    private IEnumerator LoadLevelCo(string levelName)
    {
        CleanUpScene();
        ui.EnableInGameUI(false);
        cameraEffects.SwitchToGameView();

        yield return tileAnimator.GetCurrentActiveRoutine();

        UnloadCurrentScene();
        LoadScene(levelName);
    }

    public int GetNextLevelIndex() => SceneUtility.GetBuildIndexByScenePath(currentLevelName) + 1;

    public string GetNextLevelName() => "Level_" + GetNextLevelIndex();

    public bool HasNoMoreLevels() => GetNextLevelIndex() >= SceneManager.sceneCountInBuildSettings;

    public void LoadNextLevel() => LoadLevel(GetNextLevelName());

    public void LoadMainMenu() => StartCoroutine(LoadMainMenuCo());

    public void LoadLevelFromMenu(string levelName) => StartCoroutine(LoadLevelFromMenuCo(levelName));

    public void LoadLevel(string levelName) => StartCoroutine(LoadLevelCo(levelName));

    public void RestartLevel() => StartCoroutine(LoadLevelCo(currentLevelName));

    public void UpdateCurrentGrid(GridBuilder newGrid) => currentActiveGrid = newGrid;

    // Unload a scene in the background as the current scene run asynchronously
    public void UnloadCurrentScene() => SceneManager.UnloadSceneAsync(currentLevelName);

    // Load a scene in the background as the current scene run asynchronously
    private void LoadScene(string sceneNameLoad)
    {
        currentLevelName = sceneNameLoad;
        SceneManager.LoadSceneAsync(sceneNameLoad, LoadSceneMode.Additive);
    }
}
 