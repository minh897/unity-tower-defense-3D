using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField] private float defaultMoveDuration = .1f;

    [Header("Build Slot Movement")]
    [SerializeField] private float buildSlotYOffset = .25f;

    [Header("Grid Animation Details")]
    [SerializeField] private float tileMoveDuration = .1f;
    [SerializeField] private float tileDelay = .1f;
    [SerializeField] private float moveYOffset = 5;
    [Space]
    [SerializeField] private GridBuilder mainSceneGrid;
    [SerializeField] private List<GameObject> mainMenuObjects = new();

    private bool isGridMoving;
    private Coroutine currentActiveCo;

    void Start()
    {
        ShowGrid(mainSceneGrid, true);
        CollectMainSceneObjects();
    }

    void Update()
    {
        // Do something here
    }

    public void MoveTile(Transform objToMove, Vector3 targetPosition, float? newDuration = null)
    {
        float duration = newDuration ?? defaultMoveDuration;
        StartCoroutine(MoveTileRoutine(objToMove, targetPosition, duration));
    }

    public IEnumerator MoveTileRoutine(Transform objToMove, Vector3 targetPosition, float? newDuration = null)
    {
        float time = 0;
        float duration = newDuration ?? defaultMoveDuration;

        Vector3 startPosition = objToMove.position;

        while (time < duration)
        {
            objToMove.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        objToMove.position = targetPosition;
    }

    private void ApplyOffset(List<GameObject> objectsToMove, Vector3 offset)
    {
        foreach (var obj in objectsToMove)
        {
            obj.transform.position += offset;
        }
    }

    public void ShowGrid(GridBuilder gridToMove, bool isGridShow)
    {
        List<GameObject> objectsToMove = GetObjectsToMove(gridToMove, isGridShow);

        // Only apply offset on the first time the grid was loaded
        // Subsequence times will be ignored
        if (gridToMove.IsOnFirstLoad())
            ApplyOffset(objectsToMove, new Vector3(0, -moveYOffset, 0));

        float newOffset = isGridShow ? moveYOffset : -moveYOffset;
        currentActiveCo = StartCoroutine(MoveGridRoutine(objectsToMove, newOffset));
    }

    private IEnumerator MoveGridRoutine(List<GameObject> objectsToMove, float yOffset)
    {
        isGridMoving = true;

        for (int i = 0; i < objectsToMove.Count; i++)
        {
            yield return new WaitForSeconds(tileDelay);
            Transform tile = objectsToMove[i].transform;
            Vector3 targetPosition = tile.position + new Vector3(0, yOffset, 0);
            MoveTile(tile, targetPosition, tileMoveDuration);
        }

        isGridMoving = false;
    }

    public void BringUpMainGrid(bool isMainGridShow)
    {
        ShowGrid(mainSceneGrid, isMainGridShow);
    }

    // Return a list of all extra object in the scene
    private List<GameObject> CollectExtraObject()
    {
        List<GameObject> extraObjects = new();

        // Find all objects in a scene, get their game object then add them to extraObjects list
        extraObjects.AddRange(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None).Select(component => component.gameObject));
        extraObjects.AddRange(FindObjectsByType<PlayerCastle>(FindObjectsSortMode.None).Select(component => component.gameObject));

        return extraObjects;
    }

    private List<GameObject> GetObjectsToMove(GridBuilder gridToMove, bool isStartWithTiles)
    {
        List<GameObject> objectsToMove = new();
        List<GameObject> extraObjects = CollectExtraObject();

        // If BringUpMainGrid is true, then add all the tiles to move them first
        // else, add extra objects in order to move them first
        if (isStartWithTiles)
        {
            objectsToMove.AddRange(gridToMove.GetTileSetup());
            objectsToMove.AddRange(extraObjects);
        }
        else
        {
            objectsToMove.AddRange(extraObjects);
            objectsToMove.AddRange(gridToMove.GetTileSetup());
        }

        return objectsToMove;
    }

    private void CollectMainSceneObjects()
    {
        mainMenuObjects.AddRange(mainSceneGrid.GetTileSetup());
        mainMenuObjects.AddRange(CollectExtraObject());
    }

    public void EnableMainSceneObjects(bool isEnable)
    {
        foreach (var obj in mainMenuObjects)
        {
            obj.SetActive(isEnable);
        }
    }

    public bool IsGridMoving() => isGridMoving;

    public float GetBuildOffset() => buildSlotYOffset;

    public float GetYTravelDuration() => defaultMoveDuration;

    public Coroutine GetCurrentActiveCo() => currentActiveCo;
}
