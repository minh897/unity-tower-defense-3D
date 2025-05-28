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

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;

    private float smoothTime = .1f;
    private Vector3 movementVelocity = Vector3.zero;
    private Vector3 zoomVelocity = Vector3.zero;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();

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

    public void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Determines the direction and strength of the zoom
        Vector3 zoomDirection = transform.forward * scroll * zoomSpeed;

        // Calculate new camera position after zooming
        Vector3 targetPosition = transform.position + zoomDirection;

        if (transform.position.y < minZoom && scroll > 0)
            return;

        if (transform.position.y > maxZoom && scroll < 0)
            return;

        // Smoothly move the camera from its current position to the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref zoomVelocity, smoothTime);
    }

    private float GetFocusPointDistance()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxFocusPointDistance))
            return hit.distance;

        return maxFocusPointDistance;
    }
}
