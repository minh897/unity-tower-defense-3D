using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotateSpeed;

    void Update()
    {
        float newRotationSpeed = rotateSpeed * 100;
        transform.Rotate(rotationVector * newRotationSpeed * Time.deltaTime);
    }

    public void AdjustRotationSpeed(float newSpeed)
    {
        rotateSpeed = newSpeed;
    }
}
