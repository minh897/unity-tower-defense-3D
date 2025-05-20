using System.Collections;
using UnityEngine;

public class CrossbowVisual : MonoBehaviour
{
    [SerializeField] private LineRenderer attackLine;
    [SerializeField] private float attackVisualDur = .1f;

    private TowerCrossbow mainTower;

    void Awake()
    {
        mainTower = GetComponent<TowerCrossbow>();
    }

    public void PlayAttackFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
    }

    private IEnumerator FXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        mainTower.EnableRotation(false);
        attackLine.enabled = true;

        attackLine.SetPosition(0, startPoint);
        attackLine.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDur);

        mainTower.EnableRotation(true);
        attackLine.enabled = false;
    }
}
