using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuildButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float adjustSpeed = 10f;
    [SerializeField] private float showcaseY;
    [SerializeField] private float defaultY;

    private float targetY;
    private bool canMove;

    void Update()
    {
        if (MathF.Abs(transform.position.y - targetY) > .01f && canMove)
        {
            float newPositionY = Mathf.Lerp(transform.position.y, targetY, adjustSpeed * Time.deltaTime);
            transform.position = new(transform.position.x, newPositionY, transform.position.z);
        }
    }

    public void ToggleMovement(bool isButtonActive)
    {
        canMove = isButtonActive;
        SetTargetY(defaultY);

        if (isButtonActive == false)
            SetPositionToDefault();
    }

    private void SetPositionToDefault()
    {
        transform.position = new(transform.position.x, defaultY, transform.position.z);
    }

    private void SetTargetY(float newY) => targetY = newY;

    public void OnPointerEnter(PointerEventData eventData) => SetTargetY(showcaseY);

    public void OnPointerExit(PointerEventData eventData) => SetTargetY(defaultY);
    
}
