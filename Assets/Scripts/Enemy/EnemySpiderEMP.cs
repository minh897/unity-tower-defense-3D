using UnityEngine;

public class EnemySpiderEMP : MonoBehaviour
{
    [SerializeField] private float empRadius = 2;
    [SerializeField] private float empEffectDuration = 5;
    [SerializeField] private float empSpeed = 1;
    [SerializeField] private GameObject empFX;

    private bool shouldShrink;
    private float shrinkSpeed = 3;
    private Vector3 destination;

    void Update()
    {
        MoveTowerTarget();

        if (shouldShrink)
            Shrink();
    }

    private void Shrink()
    {
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        if (transform.localScale.x <= 0.1f)
            Destroy(gameObject);
    }

    private void MoveTowerTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, empSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < .1f)
            DeactivateEMP();
    }

    public void SetupEMP(float duration, Vector3 target)
    {
        empEffectDuration = duration;
        destination = target;
    }

    void OnTriggerEnter(Collider other)
    {
        Tower tower = other.GetComponent<Tower>();

        if (tower != null)
            tower.DeactivateTower(empEffectDuration, empFX);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, empRadius);
    }

    private void DeactivateEMP() => shouldShrink = true;
}
