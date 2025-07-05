using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElement;
    [SerializeField] private Image fadeImageUI;

    [Header("UI SFX")]
    public AudioSource onHoverSFX;
    public AudioSource onClickSFX;

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

        // ActivateFadeEffect(true);
        SwitchUIElement(uiSetting.gameObject);
        SwitchUIElement(uiMainMenu.gameObject);

        if (GameManager.instance.IsTestingLevel())
            SwitchUIElement(uiInGame.gameObject);   
    }

    public void EnableMainMenuUI(bool isEnable)
    {
        if (isEnable)
            SwitchUIElement(uiMainMenu.gameObject);
        else
            SwitchUIElement(null);
    }

    public void EnableInGameUI(bool isEnable)
    {
        if (isEnable)
            SwitchUIElement(uiInGame.gameObject);
        else
        {
            uiInGame.DefaultNextWaveButonPos();
            SwitchUIElement(null);
        }
    }

    public void SwitchUIElement(GameObject uiToEnable)
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
            uiAnimator.StartChangeColor(fadeImageUI, 0, 2);
        else
            uiAnimator.StartChangeColor(fadeImageUI, 1, 2);
    }
}
