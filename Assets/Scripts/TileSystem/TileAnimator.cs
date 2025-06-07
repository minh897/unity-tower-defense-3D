using System.Collections;
using System.Collections.Generic;
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
    private bool isGridMoving;

    void Start()
    {
        ShowGrid(mainSceneGrid, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            BringUpMainGrid(true);

        if (Input.GetKeyDown(KeyCode.K))
            BringUpMainGrid(false);
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
        List<GameObject> objectsToMove = gridToMove.GetTileSetup();

        // Only apply offset on the first time the grid was loaded
        // Subsequence times will be ignored
        if (gridToMove.IsOnFirstLoad())
            ApplyOffset(objectsToMove, new Vector3(0, -moveYOffset, 0));

        float newOffset = isGridShow ? moveYOffset : -moveYOffset;
        StartCoroutine(MoveGridRoutine(objectsToMove, newOffset));
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

    public bool IsGridMoving() => isGridMoving;

    public float GetBuildOffset() => buildSlotYOffset;

    public float GetYTravelDuration() => defaultMoveDuration;
}
