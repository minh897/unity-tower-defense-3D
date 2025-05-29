using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private Transform visuals;
    [SerializeField] private LayerMask roadLayer;

    void Update()
    {
        AlignWithSlope();
    }

    private void AlignWithSlope()
    {
        if (visuals == null)
        {
            return;
        }

        // Check if a ray cast from visual position hit road layer and store the infos in RaycastHit hit
        if (Physics.Raycast(visuals.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, roadLayer))
        {
            // Calculate the rotation difference needed the visual up vector match the slope's normal
            // Multiply its by the current rotation to get the desired orientation
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // Smoothly rotate the visuals from their current rotation toward targetRotation
            visuals.rotation = Quaternion.Slerp(visuals.rotation, targetRotation, verticalRotationSpeed * Time.deltaTime);
        }
    }
}
