using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Only used for finding waypoint component

    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
