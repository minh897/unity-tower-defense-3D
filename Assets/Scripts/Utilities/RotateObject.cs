using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotateSpeed;

    void Update()
    {
        transform.Rotate(rotationVector * rotateSpeed * Time.deltaTime);
    }

    public void AdjustRotationSpeed(float newSpeed)
    {
        rotateSpeed = newSpeed;
    }
}
