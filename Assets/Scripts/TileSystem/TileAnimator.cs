using System.Collections;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField] private float yMovementDuration = .1f;

    [Header("Build Slot Movement")]
    [SerializeField] private float buildSlotYOffset = .25f;

    public void MoveTile(Transform objToMove, Vector3 targetPosition)
    {
        StartCoroutine(MoveTileRoutine(objToMove, targetPosition));
    }

    public IEnumerator MoveTileRoutine(Transform objToMove, Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = objToMove.position;

        while (time < yMovementDuration)
        {
            objToMove.position = Vector3.Lerp(startPosition, targetPosition, time / yMovementDuration);
            time += Time.deltaTime;
            yield return null;
        }

        objToMove.position = targetPosition;
    }

    public float GetBuildOffset() => buildSlotYOffset;

    public float GetYTravelDuration() => yMovementDuration;
}
