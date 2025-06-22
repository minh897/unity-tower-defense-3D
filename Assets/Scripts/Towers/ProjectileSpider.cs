using UnityEngine;

public class ProjectileSpider : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetupSpider()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
