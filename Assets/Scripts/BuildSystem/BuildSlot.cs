using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private TileAnimator tileAnimator;
    private BuildManager buildManager;
    private Vector3 defaultPosition;
    private Coroutine currentMovementUpCo;
    private Coroutine moveToDefaultCo;

    private bool canMoveTile = true;
    private bool isBuildSlotAvailable = true;

    void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();
        buildManager = FindFirstObjectByType<BuildManager>();
        defaultPosition = transform.position;
    }

    void Start()
    {
        if (isBuildSlotAvailable == false)
            transform.position += new Vector3(0, .1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isBuildSlotAvailable == false || tileAnimator.IsGridMoving())
            return;

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Prevent running the same functions 
        // from selecting the same tile over and over
        if (buildManager.GetSelectedBuildSlot() == this)
            return;

        // EnableBuildMenu need to be above SelectBuildSlot 
        // so the build menu can be enable correctly    
        buildManager.EnableBuildMenu();
        buildManager.SelectBuildSlot(this);

        MoveTileUp();

        canMoveTile = false;

        ui.uiBuildButton.GetLastSelectedButton()?.TogglePreviewVisual(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isBuildSlotAvailable == false || tileAnimator.IsGridMoving())
            return;

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
            return;

        if (canMoveTile == false)
            return;

        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isBuildSlotAvailable == false || tileAnimator.IsGridMoving())
            return;
            
        if (canMoveTile == false)
            return;

        if (currentMovementUpCo != null)
            Invoke(nameof(MoveTileDefault), tileAnimator.GetYTravelDuration());
        else
            MoveTileDefault();

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
        moveToDefaultCo = StartCoroutine(tileAnimator.MoveTileRoutine(transform, defaultPosition));
    }

    public void SnapToDefaultPosition()
    {
        if (moveToDefaultCo != null)
            StopCoroutine(moveToDefaultCo);

        transform.position = defaultPosition;
    }

    public void SetSlotAvailable(bool enable) => isBuildSlotAvailable = enable;

    public Vector3 GetBuildPosition(float yPosOffset) => defaultPosition + new Vector3(0, yPosOffset);
}
