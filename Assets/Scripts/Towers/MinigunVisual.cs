using System.Collections;
using UnityEngine;

public class MinigunVisual : MonoBehaviour
{
    [Header("Visual Details")]
    [SerializeField] private float recoilOffset = -.2f;
    [SerializeField] private float recoverSpeed = .25f;
    [SerializeField] private ParticleSystem gunFlashVFX;

    public void ReCoilGun(Transform gunPoint)
    {
        PlayVFXOnAttack(gunPoint.position);
        StartCoroutine(RecoilCo(gunPoint));
    }

    private void PlayVFXOnAttack(Vector3 gunPointPosition)
    {
        gunFlashVFX.transform.position = gunPointPosition;
        gunFlashVFX.Play();
    }

    private IEnumerator RecoilCo(Transform gunPoint)
    {
        Transform objectToMove = gunPoint.transform.parent;
        Vector3 ogPosition = objectToMove.localPosition;
        Vector3 recoilPosition = ogPosition + new Vector3(0, 0, recoilOffset);

        objectToMove.localPosition = recoilPosition;

        while (objectToMove.localPosition != ogPosition)
        {
            objectToMove.localPosition = Vector3.MoveTowards(objectToMove.localPosition, ogPosition, recoverSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
