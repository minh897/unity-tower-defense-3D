using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    private bool towerAttacksForward;
    private float attackRange;

    private TowerAttackRangeDisplay attackDisplay;
    private ForwardAttackDisplay forwardAttack;
    private MeshRenderer[] meshRenderers;

    private List<System.Type> compToKeep = new();

    public void SetupTowerPreview(GameObject towerToBuild)
    {
        Tower tower = towerToBuild.GetComponent<Tower>();

        // Attach the TowerAttackRangeDisplay component to this object
        // then assign it to attackDisplay variable
        attackDisplay = transform.AddComponent<TowerAttackRangeDisplay>();
        forwardAttack = tower.GetComponent<ForwardAttackDisplay>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        attackRange = tower.GetAttackRange();
        towerAttacksForward = tower.isAttackForward;

        SecureComponents();
        MakeAllMeshTransparent();
        DestroyExtraComponents();

        gameObject.SetActive(false);
    }

    public void ShowPreview(bool isPreviewShow, Vector3 previewPosition)
    {
        transform.position = previewPosition;

        if (towerAttacksForward == false)
            attackDisplay.CreateCircle(isPreviewShow, attackRange);
        else
            forwardAttack.CreateLines(isPreviewShow, attackRange);
    }

    private void DestroyExtraComponents()
    {
        Component[] components = GetComponents<Component>();

        foreach (var componentToCheck in components)
        {
            if (ComponentSecured(componentToCheck) == false)
                Destroy(componentToCheck);
        }
    }

    private void SecureComponents()
    {
        compToKeep.Add(typeof(TowerPreview));
        compToKeep.Add(typeof(TowerAttackRangeDisplay));
        compToKeep.Add(typeof(Transform));
        compToKeep.Add(typeof(LineRenderer));
        compToKeep.Add(typeof(ForwardAttackDisplay));
    }

    private bool ComponentSecured(Component compToCheck)
    {
        return compToKeep.Contains(compToCheck.GetType());
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
