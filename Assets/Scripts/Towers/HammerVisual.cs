using System.Collections;
using UnityEngine;

public class HammerVisual : MonoBehaviour
{
    [Header("VFX Details")]
    [SerializeField] private ParticleSystem[] vfxList;
    [Space]

    [Header("Hammer Details")]
    [SerializeField] private Transform hammer;
    [SerializeField] private Transform hammerHolder;
    [Space]

    [SerializeField] private Transform sideWire;
    [SerializeField] private Transform sideHandle;
    [Space]

    [SerializeField] private RotateObject valeRotation;

    [Header("Attack and Release Details")]
    [SerializeField] private float attackOffsetY;
    [SerializeField] private float attackDuration;
    [SerializeField] private float reloadDuration;

    private TowerHammer towerHammer;

    void Awake()
    {
        towerHammer = GetComponent<TowerHammer>();
        reloadDuration = towerHammer.GetAttackCooldown() - attackDuration;
    }

    public void PlayAttackAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(HammerAttackCo());
    }

    public IEnumerator HammerAttackCo()
    {
        valeRotation.AdjustRotationSpeed(250);

        StartCoroutine(ChangePositionCo(hammer, -attackOffsetY, attackDuration));
        StartCoroutine(ChangeScaleCo(hammerHolder, 7, attackDuration));

        StartCoroutine(ChangePositionCo(sideHandle, .45f, attackDuration));
        StartCoroutine(ChangeScaleCo(sideWire, .1f, attackDuration));

        yield return new WaitForSeconds(attackDuration);

        PlayVFXs();

        valeRotation.AdjustRotationSpeed(30);

        StartCoroutine(ChangePositionCo(hammer, attackOffsetY, reloadDuration));
        StartCoroutine(ChangeScaleCo(hammerHolder, 1, reloadDuration));

        StartCoroutine(ChangePositionCo(sideHandle, -.45f, reloadDuration));
        StartCoroutine(ChangeScaleCo(sideWire, 1f, reloadDuration));
    }

    private void PlayVFXs()
    {
        foreach (var p in vfxList)
        {
            p.Play();
        }
    }

    public IEnumerator ChangePositionCo(Transform obj, float yOffset, float duration = .1f)
    {
        float time = 0;

        Vector3 initialPosition = obj.localPosition;
        Vector3 targetPosition = new(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);

        while (time < duration)
        {
            obj.localPosition = Vector3.Lerp(initialPosition, targetPosition, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        obj.localPosition = targetPosition;
    }

    public IEnumerator ChangeScaleCo(Transform obj, float newScale, float duration = .25f)
    {
        float time = 0;

        Vector3 initialScale = obj.localScale;
        Vector3 targetScale = new(1, newScale, 1);

        while (time < duration)
        {
            obj.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        obj.localScale = targetScale;
    }
}
