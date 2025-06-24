using UnityEngine;

public class ProjectileHarpoonLink : MonoBehaviour
{
    private LineRenderer lr;
    private MeshRenderer mesh;
    private ParticleSystem vfx;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        mesh = GetComponentInChildren<MeshRenderer>();
        vfx = GetComponentInChildren<ParticleSystem>();

        // Two connection points, one for the current chain and one for the next chain
        lr.positionCount = 2;

        EnableLinK(false, transform.position);
    }

    // Make the chain visual visible
    public void EnableLinK(bool enable, Vector3 newPosition)
    {
        mesh.enabled = enable;
        transform.position = newPosition;
    }

    public void UpdateLineRenderer(ProjectileHarpoonLink startPoint, ProjectileHarpoonLink endPoint)
    {
        // Only enable LineRenderer when both start and end points of the chain is enable
        lr.enabled = startPoint.CurrentlyActive() && endPoint.CurrentlyActive();

        if (lr.enabled == false)
            return;

        lr.SetPosition(0, startPoint.transform.position);
        lr.SetPosition(1, endPoint.transform.position);
    }

    private void EnableVFX(bool enable)
    {
        if (enable && vfx.isPlaying == false)
            vfx.Play();
        else
            vfx.Stop();
    }

    // Return status of MeshRenderer of both start and end points
    public bool CurrentlyActive() => mesh.enabled;
}
