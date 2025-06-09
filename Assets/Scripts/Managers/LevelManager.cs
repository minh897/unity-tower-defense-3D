using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private UI ui;
    private TileAnimator tileAnimator;
    private GridBuilder currentActiveGrid;
    private string currentSceneName;

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            StartCoroutine(LoadLevelFromMenuCo("Level_1"));
        if (Input.GetKeyDown(KeyCode.K))
            StartCoroutine(LoadMainMenuCo());
    }

    private void EleminateAllEnemies()
    {
        Enemy[] enemiesArray = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemiesArray)
        {
            enemy.DestroyEnemy();
        }
    }

    private void EleminateAllTowers()
    {
        Tower[] towersArray = FindObjectsByType<Tower>(FindObjectsSortMode.None);

        foreach (Tower tower in towersArray)
        {
            Destroy(tower.gameObject);
        }
    }

    private IEnumerator LoadLevelFromMenuCo(string levelName)
    {
        tileAnimator.ShowMainGrid(false);
        ui.EnableMainMenuUI(false);

        yield return tileAnimator.GetCurrentActiveRoutine();

        tileAnimator.EnableMainSceneObjects(false);

        LoadScene(levelName);
    }

    private IEnumerator LoadMainMenuCo()
    {
        // Delay until this GetCurrentActiveRoutine coroutine is finished
        yield return tileAnimator.GetCurrentActiveRoutine();

        UnloadCurrentScene();
        tileAnimator.EnableMainSceneObjects(true);
        tileAnimator.ShowMainGrid(true);

        yield return tileAnimator.GetCurrentActiveRoutine();

        ui.EnableMainMenuUI(true);
    }

    public void UpdateCurrentGrid(GridBuilder newGrid) => currentActiveGrid = newGrid;

    // Unload a scene in the background as the current scene run asynchronously
    public void UnloadCurrentScene() => SceneManager.UnloadSceneAsync(currentSceneName);

    // Load a scene in the background as the current scene run asynchronously
    private void LoadScene(string sceneNameLoad)
    {
        currentSceneName = sceneNameLoad;
        SceneManager.LoadSceneAsync(sceneNameLoad, LoadSceneMode.Additive);
    }

}
 