using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElement;

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
