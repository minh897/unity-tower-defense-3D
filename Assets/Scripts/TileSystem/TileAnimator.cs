using System.Collections;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField] private float yMovementDuration = .1f;

    private float testYOffset = .25f;
    public Transform testObject;

    [ContextMenu("Move Tile")]
    public void TestMovementOfObj()
    {
        Vector3 targetPosition = testObject.position + new Vector3(0, testYOffset);
        MoveTile(testObject, targetPosition);
    }

    public void MoveTile(Transform objToMove, Vector3 targetPosition)
    {
        StartCoroutine(MoveTileRoutine(objToMove, targetPosition));
    }

    private IEnumerator MoveTileRoutine(Transform objToMove, Vector3 targetPosition)
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
}
