using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElement;
    [SerializeField] private Image fadeImageUI;

    private UISetting uISetting;
    private UIMainMenu uIMainMenu;
    private UIInGame uIInGame;
    private UIAnimator uIAnimator;

    void Awake()
    {
        uISetting = GetComponentInChildren<UISetting>(true);
        uIMainMenu = GetComponentInChildren<UIMainMenu>(true);
        uIInGame = GetComponentInChildren<UIInGame>(true);
        uIAnimator = GetComponent<UIAnimator>();

        SwitchUIElemnt(uISetting.gameObject);
        // SwitchUIElemnt(uIMainMenu.gameObject);
        SwitchUIElemnt(uIInGame.gameObject);

        // ActivateFadeEffect(true);
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

    public void ActivateFadeEffect(bool fadeIn)
    {
        if (fadeIn)
            uIAnimator.ChangeColor(fadeImageUI, 0, 2);
        else
            uIAnimator.ChangeColor(fadeImageUI, 1, 2);
    }
}
