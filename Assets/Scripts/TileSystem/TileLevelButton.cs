using UnityEngine;
using UnityEngine.EventSystems;

public class TileLevelButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int levelIndex;

    private bool canClick;
    private bool canMove;
    private Vector3 defaultPosition;
    private Coroutine currentMoveCo;
    private Coroutine moveToDefaultCo;
    private LevelManager levelManager;
    private TileAnimator tileAnimator;

    void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();

        defaultPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canClick == false)
            return;
            
        Debug.Log("Loading Level: " + levelIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canMove == false)
            return;

        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canMove == false)
            return;

        if (currentMoveCo != null)
            Invoke(nameof(MoveTileDefault), tileAnimator.GetYTravelDuration());
        else
            MoveTileDefault();
    }

    void OnEnable()
    {
        canMove = true;
    }

    private void MoveTileUp()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, tileAnimator.GetBuildOffset(), 0);
        currentMoveCo = StartCoroutine(tileAnimator.MoveTileCo(transform, targetPosition));
    }

    private void MoveTileDefault()
    {
        moveToDefaultCo = StartCoroutine(tileAnimator.MoveTileCo(transform, defaultPosition));
    }

    public void EnableClick(bool isEnable) => canClick = isEnable;
}
