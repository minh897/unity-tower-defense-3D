using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TowerAttackRangeDisplay : MonoBehaviour
{
    [SerializeField] private float range;
    private int segments = 50; // Amount of dots used to create a circle

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1; // Add extra point to close the circle
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
    }

    public void ShowAttackRange(bool isRangeShow, float newRange, Vector3 newCenter)
    {
        lineRenderer.enabled = isRangeShow;

        if (isRangeShow == false)
            return;

        transform.position = newCenter;
        range = newRange;

        CreateCircle();
    }

    private void CreateCircle()
    {
        float angle = 0;
        Vector3 center = transform.position;

        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

            lineRenderer.SetPosition(i, new Vector3(x + center.x, center.y, z + center.z));
            angle += 360 / segments;
        }

        lineRenderer.SetPosition(segments, lineRenderer.GetPosition(0));
    }
}
