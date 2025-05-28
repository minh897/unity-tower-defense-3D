using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float maxFocusPointDistance = 15f;
    [SerializeField] private Transform focusPoint;
    [Space]
    [SerializeField] private float minPitch = 5f;
    [SerializeField] private float maxPitch = 85f;
    private float pitch;

    private float smoothTime = .1f;
    private Vector3 movementVelocity = Vector3.zero;

    void Update()
    {
        HandleMovement();
        HandleRotation();

        focusPoint.position = transform.position + transform.forward * GetFocusPointDistance();
    }

    public void HandleMovement()
    {
        Vector3 targetPosition = transform.position;
        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        if (vInput > 0)
            targetPosition += flatForward * moveSpeed * Time.deltaTime;
        if (vInput < 0)
            targetPosition -= flatForward * moveSpeed * Time.deltaTime;

        if (hInput > 0)
            targetPosition += transform.right * moveSpeed * Time.deltaTime;
        if (hInput < 0)
            targetPosition -= transform.right * moveSpeed * Time.deltaTime;


        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref movementVelocity, smoothTime);
    }

    public void HandleRotation()
    {
        // Check if the right mouse button is being hold
        if (Input.GetMouseButton(1))
        {
            float hRotation = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float vRotation = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            // Adjusts the vertical angle and Clamp it to keep it in safe ranges
            pitch = Mathf.Clamp(pitch - vRotation, minPitch, maxPitch);

            // Rotate horizontally around the focus point using the world's up axis
            transform.RotateAround(focusPoint.position, Vector3.up, hRotation);

            // Rotate vertically around the point using local right axis
            // Smoothly adjust the angle by the difference between desired pitch and current x rotation
            transform.RotateAround(focusPoint.position, transform.right, pitch - transform.eulerAngles.x);

            // Make the camera always look at the focus point
            transform.LookAt(focusPoint);
        }
    }

    private float GetFocusPointDistance()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxFocusPointDistance))
            return hit.distance;

        return maxFocusPointDistance;
    }
}
