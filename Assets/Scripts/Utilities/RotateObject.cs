using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotateSpeed;

    void Update()
    {
        transform.Rotate(rotationVector * rotateSpeed * Time.deltaTime);    
    }
}
