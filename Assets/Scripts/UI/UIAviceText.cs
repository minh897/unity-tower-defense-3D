using TMPro;
using UnityEngine;

public class UIAviceText : MonoBehaviour
{
    private TextMeshProUGUI adviceText;

    [SerializeField] private string[] advices;

    void OnEnable()
    {
        if (adviceText == null)
            adviceText = GetComponent<TextMeshProUGUI>();

        int randomIndex = Random.Range(0, advices.Length);
        adviceText.text = advices[randomIndex];
    }
}
