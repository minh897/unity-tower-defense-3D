using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private TileAnimator tileAnimator;
    private BuildManager buildManager;
    private Vector3 defaultPosition;
    private Coroutine currentMovementUpCo;

    private bool canMoveTile = true;

    void Awake()
    {
        tileAnimator = FindFirstObjectByType<TileAnimator>();
        buildManager = FindFirstObjectByType<BuildManager>();
        defaultPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
            
        buildManager.SelectBuildSlot(this);
        MoveTileUp();
        canMoveTile = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
            return;

        if (canMoveTile == false)
            return;

        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canMoveTile == false)
            return;

        if (currentMovementUpCo != null)
        {
            Invoke(nameof(MoveTileDefault), tileAnimator.GetYTravelDuration());
        }
        else
        {
            MoveTileDefault();
        }

        MoveTileDefault();
    }

    public void UnSelectTile()
    {
        MoveTileDefault();
        canMoveTile = true;
    }

    private void MoveTileUp()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, tileAnimator.GetBuildOffset(), 0);
        currentMovementUpCo = StartCoroutine(tileAnimator.MoveTileRoutine(transform, targetPosition));
    }

    private void MoveTileDefault()
    {
        tileAnimator.MoveTile(transform, defaultPosition);
    }
}
