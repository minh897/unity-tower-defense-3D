using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private UI ui;
    private TileAnimator tileAnimator;
    private CameraEffects cameraEffects;
    private GridBuilder currentActiveGrid;

    public string currentSceneName { get; private set; }

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        currentActiveGrid = FindFirstObjectByType<GridBuilder>();
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
            currentActiveGrid.ClearNavMeshData();
    }

    private IEnumerator LoadLevelFromMenuCo()
    {
        CleanUpScene();
        ui.EnableMainMenuUI(false);
        cameraEffects.SwitchToGameView();

        yield return tileAnimator.GetCurrentActiveRoutine();

        ui.EnableInGameUI(true);
        cameraEffects.EnableCameraEffect();

        GameManager.instance.PrepareLevel(GameManager.instance.activeWaveManager);
    }

    private IEnumerator LoadMainMenuCo()
    {
        CleanUpScene();
        ui.EnableInGameUI(false);

        yield return cameraEffects.GetActiveCameraCo();

        ui.EnableMainMenuUI(true);
    }

    private IEnumerator LoadLevelCo(string sceneName)
    {
        CleanUpScene();
        ui.EnableInGameUI(false);
        cameraEffects.SwitchToGameView();

        yield return tileAnimator.GetCurrentActiveRoutine();

        UnloadCurrentScene();
        LoadScene(sceneName);
    }

    public void LoadMainMenu() => StartCoroutine(LoadMainMenuCo());

    public void LoadLevelFromMenu() => StartCoroutine(LoadLevelFromMenuCo());

    public void RestartGame() => StartCoroutine(LoadLevelCo(currentSceneName));

    public void UpdateCurrentGrid(GridBuilder newGrid) => currentActiveGrid = newGrid;

    // Unload a scene in the background as the current scene run asynchronously
    public void UnloadCurrentScene() => SceneManager.UnloadSceneAsync(currentSceneName);

    // Load a scene in the background as the current scene run asynchronously
    private void LoadScene(string sceneName)
    {
        currentSceneName = sceneName;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
 