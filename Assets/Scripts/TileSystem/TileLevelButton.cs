using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileLevelButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int levelIndex;

    private bool canClick;
    private bool unlocked;

    private Vector3 defaultPosition;
    private Coroutine currentMoveCo;
    private Coroutine moveToDefaultCo;
    private LevelManager levelManager;
    private TileAnimator tileAnimator;

    private TextMeshPro myText => GetComponentInChildren<TextMeshPro>();

    void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();

        defaultPosition = transform.position;
        CheckIfLevelUnlocked();
    }

    public void CheckIfLevelUnlocked()
    {
        if (levelIndex == 1)
            PlayerPrefs.SetInt("Level_1" + "unlocked", 1);

        unlocked = PlayerPrefs.GetInt("Level_" + levelIndex + "unlocked", 0) == 1;
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (unlocked == false)
            myText.text = "Locked";
        else
            myText.text = "Level " + levelIndex;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canClick == false)
            return;

        if (unlocked == false)
        {
            Debug.Log("This level is locked");
            // play sound effect
            return;
        }

        transform.position = defaultPosition;
        levelManager.LoadLevelFromMenu("Level_" + levelIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tileAnimator.IsGridMoving())
            return;

        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tileAnimator.IsGridMoving())
            return;

        if (currentMoveCo != null)
            Invoke(nameof(MoveTileDefault), tileAnimator.GetYTravelDuration());
        else
            MoveTileDefault();
    }

    void OnValidate()
    {
        levelIndex = transform.GetSiblingIndex() + 1;

        if (myText != null)
            myText.text = "Level " + levelIndex;
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
