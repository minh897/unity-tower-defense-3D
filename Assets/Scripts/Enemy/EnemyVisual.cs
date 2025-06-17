using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] protected Transform visuals;
    [SerializeField] private LayerMask roadLayer;

    [Header("Transparency Details")]
    [SerializeField] private Material transparentMat;
    private List<Material> originalMats;
    private MeshRenderer[] myMeshes;

    protected virtual void Awake()
    {
        CollectDefaultMat();
    }

    protected virtual void Start()
    {
        // Do something
    }

    protected virtual void Update()
    {
        AlignWithSlope();

        if (Input.GetKeyDown(KeyCode.X))
            MakeTransparent(true);

        if (Input.GetKeyDown(KeyCode.C))
            MakeTransparent(false);
    }

    public void MakeTransparent(bool isTransparent)
    {
        for (int i = 0; i < myMeshes.Length; i++)
        {
            myMeshes[i].material = isTransparent ? transparentMat : originalMats[i];
        }
    }

    protected void CollectDefaultMat()
    {
        myMeshes = GetComponentsInChildren<MeshRenderer>();
        originalMats = new();

        foreach (var renderer in myMeshes)
        {
            originalMats.Add(renderer.material);
        }
    }

    private void AlignWithSlope()
    {
        if (visuals == null)
        {
            return;
        }

        // Check if a ray cast from visual position hit road layer and store the infos in RaycastHit hit
        if (Physics.Raycast(visuals.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, roadLayer))
        {
            // Calculate the rotation difference needed the visual up vector match the slope's normal
            // Multiply its by the current rotation to get the desired orientation
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // Smoothly rotate the visuals from their current rotation toward targetRotation
            visuals.rotation = Quaternion.Slerp(visuals.rotation, targetRotation, verticalRotationSpeed * Time.deltaTime);
        }
    }
}
