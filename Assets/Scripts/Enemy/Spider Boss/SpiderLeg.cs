using System.Collections;
using UnityEngine;

public class SpiderLeg : MonoBehaviour
{
    [Header("Movement Details")]
    [SerializeField] private float legMoveSpeed = 2.5f;
    [SerializeField] private float moveThreshold = .45f;
    private bool canMove = true;
    private bool shouldMove;
    private Coroutine moveCo;
    [Space]

    [Header("Reference Details")]
    [SerializeField] private Vector3 placementOffset;
    [SerializeField] private SpiderLeg oppositeLeg;
    [SerializeField] private SpiderLegRef legRef;
    [SerializeField] private Transform actualTarget;
    [SerializeField] private Transform bottomLeg;
    [SerializeField] private Transform worldTargetRef;
    private EnemySpiderVisual spiderVisual;

    void Awake()
    {
        spiderVisual = GetComponentInParent<EnemySpiderVisual>();
        worldTargetRef = Instantiate(worldTargetRef, actualTarget.position, Quaternion.identity).transform;

        worldTargetRef.gameObject.name = legRef.gameObject.name + "_world";
        legMoveSpeed = spiderVisual.legSpeed;
    }

    public void UpdateLeg()
    {
        actualTarget.position = worldTargetRef.position + placementOffset;

        // Only move when the distance between leg contact point and target position is exceeded the desired threshold
        shouldMove = Vector3.Distance(worldTargetRef.position, legRef.ContactPoint()) > moveThreshold;

        if (bottomLeg != null)
            bottomLeg.forward = Vector3.down;

        if (shouldMove && canMove)
        {
            if (moveCo != null)
                StopCoroutine(moveCo);

            moveCo = StartCoroutine(LegMoveCo());
        }
    }

    public void SpeedUpLeg()
    {
        StartCoroutine(SpeedUpLegCo());
    }

    private IEnumerator LegMoveCo()
    {
        oppositeLeg.CanMove(false);

        while (Vector3.Distance(worldTargetRef.position, legRef.ContactPoint()) > 0.1f)
        {
            worldTargetRef.position = Vector3.MoveTowards(worldTargetRef.position, legRef.ContactPoint(), legMoveSpeed * Time.deltaTime);
            yield return null;
        }

        oppositeLeg.CanMove(true);
    }

    private IEnumerator SpeedUpLegCo()
    {
        legMoveSpeed = spiderVisual.increaseLegSpeed;

        yield return new WaitForSeconds(1f);

        legMoveSpeed = spiderVisual.legSpeed;
    }

    private void CanMove(bool enableMovement) => canMove = enableMovement;
}
