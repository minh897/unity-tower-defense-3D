using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [Header("Tower Setup")]
    [SerializeField] private Transform towerHead;
    [SerializeField] private float rotationSpeed;


    void Update()
    {
        RotateTowardsEnemy();
    }

    private void RotateTowardsEnemy()
    {
        // Calculate the vector direction from the tower head to the current enemy's position
        Vector3 directionToTarget = currentEnemy.position - towerHead.position;

        // Create a new rotation that look in the direction of the target
        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        // Calculate the in-between rotation from the current to the target orientation
        // Then convert it to Euler angles (rotations in degrees around each axis)
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, newRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Convert the Euler angle rotation to Quaternion and apply it to the tower head rotation 
        towerHead.rotation = Quaternion.Euler(rotation);

        Debug.DrawRay(towerHead.position, directionToTarget, Color.green);
    }
}
