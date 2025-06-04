using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    private MeshRenderer tileMeshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter tileMeshFilter => GetComponent<MeshFilter>();
    private Collider tileCollider => GetComponent<Collider>();
    private NavMeshSurface tileNavMesh => GetComponentInParent<NavMeshSurface>(true);
    private TileSetHolder tileSetHolder => GetComponentInParent<TileSetHolder>(true);

    public void SwitchTile(GameObject referenceTile)
    {
        gameObject.name = referenceTile.name;

        TileSlot newTile = referenceTile.GetComponent<TileSlot>();

        tileMeshRenderer.material = newTile.GetMaterial();
        tileMeshFilter.mesh = newTile.GetMesh();

        UpdateCollider(newTile.GetCollider());
        UpdateChildrenObject(newTile);
        UpdateLayer(referenceTile);
        UpdateNavMesh();

        CheckForBuildSlot(referenceTile);
    }

    public List<GameObject> GetAllChildren()
    {
        List<GameObject> children = new();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }

    // Change the current collider to a new one when switching different tile
    public void UpdateCollider(Collider referenceCollider)
    {
        DestroyImmediate(tileCollider);

        if (referenceCollider is BoxCollider)
        {
            BoxCollider reference = referenceCollider.GetComponent<BoxCollider>();
            BoxCollider newCollider = transform.AddComponent<BoxCollider>();

            newCollider.center = reference.center;
            newCollider.size = reference.size;
        }

        if (referenceCollider is MeshCollider)
        {
            MeshCollider reference = referenceCollider.GetComponent<MeshCollider>();
            MeshCollider newCollider = transform.AddComponent<MeshCollider>();

            newCollider.sharedMesh = reference.sharedMesh;
            newCollider.convex = reference.convex;
        }
    }

    private void UpdateChildrenObject(TileSlot newTile)
    {
        foreach (GameObject child in GetAllChildren())
        {
            // Use this because Detroy doesn't work in Editor
            DestroyImmediate(child);
        }

        // Find every child game object in the new tile
        // and create them in the selected tile
        foreach (GameObject obj in newTile.GetAllChildren())
        {
            Instantiate(obj, transform);
        }
    }

    private void CheckForBuildSlot(GameObject refTile)
    {
        BuildSlot buildSlot = GetComponent<BuildSlot>();

        if (refTile != tileSetHolder.tileField)
        {
            if (buildSlot != null)
                DestroyImmediate(buildSlot);
        }
        else
        {
            if (buildSlot == null)
                gameObject.AddComponent<BuildSlot>();
        }
    }

    public void AdjustYRotation(int dir)
    {
        transform.Rotate(0, 90 * dir, 0);
        UpdateNavMesh();
    }

    public void AdjustYPosition(int verticalDir)
    {
        transform.position += new Vector3(0, 0.1f * verticalDir, 0);
        UpdateNavMesh();
    }

    public Material GetMaterial() => tileMeshRenderer.sharedMaterial;

    public Mesh GetMesh() => tileMeshFilter.sharedMesh;

    public Collider GetCollider() => tileCollider;

    private void UpdateLayer(GameObject referenceObj) => gameObject.layer = referenceObj.layer;

    private void UpdateNavMesh() => tileNavMesh.BuildNavMesh();
}
