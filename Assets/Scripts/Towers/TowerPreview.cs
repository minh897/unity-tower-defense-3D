using Unity.VisualScripting;
using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    private float attackRange;

    private Tower myTower;
    private TowerAttackRangeDisplay attackDisplay;
    private MeshRenderer[] meshRenderers;

    void Awake()
    {
        // Attach the TowerAttackRangeDisplay component to this object
        // then assign it to attackDisplay variable
        attackDisplay = transform.AddComponent<TowerAttackRangeDisplay>();
        myTower = GetComponent<Tower>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        attackRange = myTower.GetAttackRange();

        MakeAllMeshTransparent();
        DestroyExtraComponents();
    }

    public void ShowPreview(bool isPreviewShow, Vector3 previewPosition)
    {
        transform.position = previewPosition;
        attackDisplay.CreateCircle(isPreviewShow, attackRange);
    }

    private void DestroyExtraComponents()
    {
        if (myTower != null)
        {
            CrossbowVisual crossbowVisual = GetComponent<CrossbowVisual>();
            Destroy(crossbowVisual);
            Destroy(myTower);
        }
    }

    private void MakeAllMeshTransparent()
    {
        Material previewMat = FindFirstObjectByType<BuildManager>().GetBuildPreviewMaterial();

        foreach (var mesh in meshRenderers)
        {
            mesh.material = previewMat;
        }
    }
}
