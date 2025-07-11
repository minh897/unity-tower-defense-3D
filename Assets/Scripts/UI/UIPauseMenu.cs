using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] pauseUIElements;

    private UI ui;
    private UIInGame inGameUI;

    void Awake()
    {
        ui = GetComponentInParent<UI>();
        inGameUI = ui.GetComponentInChildren<UIInGame>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ui.SwitchUIElement(inGameUI.gameObject);
    }

    public void SwitchPauseUIElements(GameObject uiElement)
    {
        foreach (GameObject obj in pauseUIElements)
        {
            obj.SetActive(false);
        }

        uiElement.SetActive(true);
    }

    void OnEnable()
    {
        Time.timeScale = 0;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }
}
