using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElement;
    [SerializeField] private Image fadeImageUI;

    private UISetting uiSetting;
    private UIMainMenu uiMainMenu;

    public UIInGame uiInGame { get; private set; }
    public UIAnimator uiAnimator { get; private set; }
    public UIBuildButtonsHolder uiBuildButton { get; private set; }

    void Awake()
    {
        uiBuildButton = GetComponentInChildren<UIBuildButtonsHolder>(true);
        uiSetting = GetComponentInChildren<UISetting>(true);
        uiMainMenu = GetComponentInChildren<UIMainMenu>(true);
        uiInGame = GetComponentInChildren<UIInGame>(true);
        uiAnimator = GetComponent<UIAnimator>();

        SwitchUIElemnt(uiSetting.gameObject);
        SwitchUIElemnt(uiMainMenu.gameObject);
        // SwitchUIElemnt(uiInGame.gameObject);

        // ActivateFadeEffect(true);
    }

    public void EnableMainMenuUI(bool isEnable)
    {
        if (isEnable)
            SwitchUIElemnt(uiMainMenu.gameObject);
        else
            SwitchUIElemnt(null);
    }

    public void EnableInGameUI(bool isEnable)
    {
        if (isEnable)
            SwitchUIElemnt(uiInGame.gameObject);
        else
            SwitchUIElemnt(null);
    }

    public void SwitchUIElemnt(GameObject uiToEnable)
    {
        foreach (GameObject element in uiElement)
        {
            element.SetActive(false);
        }

        if (uiToEnable != null)
            uiToEnable.SetActive(true);
    }

    public void QuitToDesktop()
    {
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }

    public void ActivateFadeEffect(bool fadeIn)
    {
        if (fadeIn)
            uiAnimator.ChangeColor(fadeImageUI, 0, 2);
        else
            uiAnimator.ChangeColor(fadeImageUI, 1, 2);
    }
}
