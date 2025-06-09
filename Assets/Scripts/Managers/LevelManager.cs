using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private UI ui;
    private TileAnimator tileAnimator;

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            StartCoroutine(LoadLevelRoutine());
    }

    // Load a scene in the background as the current scene run asynchronously
    private void LoadScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

    private IEnumerator LoadLevelRoutine()
    {
        tileAnimator.BringUpMainGrid(false);
        ui.EnableMainMenuUI(false);

        yield return tileAnimator.GetCurrentActiveCo();

        tileAnimator.EnableMainSceneObjects(false);

        LoadScene("Level_1");
    }
}
