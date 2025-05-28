using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;

    private float smoothTime = .1f;
    private Vector3 movementVelocity = Vector3.zero;

    void Update()
    {
        HandleMovement();
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
}
