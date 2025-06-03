using TMPro;
using UnityEngine;

public class UITextBlinkEffect : MonoBehaviour
{
    [SerializeField] private float changeValueSpeed;

    private float targetAlpha;
    private bool canBlink;
    private TextMeshProUGUI myText;

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (canBlink == false)
            return;
        
        if (Mathf.Abs(myText.color.a - targetAlpha) > .01f)
        {
            // Gradually change the current alpha color to the target alpha color
            // If target alpha is 1, increase myText alpha color
            // If target alpha is 0, decrease myText alpha color
            float newAlpha = Mathf.Lerp(myText.color.a, targetAlpha, changeValueSpeed * Time.deltaTime);
            ChangeColorAlpha(newAlpha);
        }
        else
        {
            ChangeTargetAlpha();
        }
    }

    public void ToggleBlinkEffect(bool enable)
    {
        canBlink = enable;

        if (canBlink == false)
            ChangeColorAlpha(1);
    }

    private void ChangeColorAlpha(float newAlpha)
    {
        Color myColor = myText.color;
        myText.color = new(myColor.r, myColor.g, myColor.b, newAlpha);
    }

    private void ChangeTargetAlpha() => targetAlpha = (targetAlpha == 1) ? 0 : 1;
}
