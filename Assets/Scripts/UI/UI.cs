using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElement;

    private UISetting uISetting;
    private UIMainMenu uIMainMenu;

    void Awake()
    {
        uISetting = GetComponentInChildren<UISetting>(true);
        uIMainMenu = GetComponentInChildren<UIMainMenu>(true);

        SwitchUIElemnt(uISetting.gameObject);
        SwitchUIElemnt(uIMainMenu.gameObject);
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
