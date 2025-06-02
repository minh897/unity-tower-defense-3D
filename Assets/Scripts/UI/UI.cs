using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElement;

    private UISetting uISetting;
    private UIMainMenu uIMainMenu;
    private UIInGame uIInGame;

    void Awake()
    {
        uISetting = GetComponentInChildren<UISetting>(true);
        uIMainMenu = GetComponentInChildren<UIMainMenu>(true);
        uIInGame = GetComponentInChildren<UIInGame>(true);

        SwitchUIElemnt(uISetting.gameObject);
        // SwitchUIElemnt(uIMainMenu.gameObject);
        SwitchUIElemnt(uIInGame.gameObject);
    }

    public void SwitchUIElemnt(GameObject uiToEnable)
    {
        foreach (GameObject element in uiElement)
        {
            element.SetActive(false);
        }

        uiToEnable.SetActive(true);
    }

    public void QuitToDesktop()
    {
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}
