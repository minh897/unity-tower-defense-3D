using UnityEngine;


public class Camera : MonoBehaviour
{
    [SerializeField] private float keyboardMoveSpeed = 90f;
    private Vector3 movementVelocity = Vector3.zero;
    private float smoothTime = .1f;

    void FixedUpdate()
    {
        HandleKeyBoardMovement();
    }

    private void HandleKeyBoardMovement()
    {
        Vector3 targetPosition = transform.position;
        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        if (vInput == 0 && hInput == 0)
            return;

        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        if (vInput > 0)
            targetPosition += flatForward * keyboardMoveSpeed * Time.deltaTime;
        if (vInput < 0)
            targetPosition -= flatForward * keyboardMoveSpeed * Time.deltaTime;

        if (hInput > 0)
            targetPosition += transform.right * keyboardMoveSpeed * Time.deltaTime;
        if (hInput < 0)
            targetPosition -= transform.right * keyboardMoveSpeed * Time.deltaTime;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref movementVelocity, smoothTime);
    }
}
