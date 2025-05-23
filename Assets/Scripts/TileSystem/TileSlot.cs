using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    private MeshRenderer tileMeshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter tileMeshFilter => GetComponent<MeshFilter>();
    private Collider tileCollider => GetComponent<Collider>();


    public Material GetMaterial() => tileMeshRenderer.sharedMaterial;

    public Mesh GetMesh() => tileMeshFilter.sharedMesh;

    public Collider GetCollider() => tileCollider;


    public void SwitchTile(GameObject referenceTile)
    {
        TileSlot newTile = referenceTile.GetComponent<TileSlot>();

        tileMeshRenderer.material = newTile.GetMaterial();
        tileMeshFilter.mesh = newTile.GetMesh();

        UpdateCollider(newTile.GetCollider());

        foreach (GameObject child in GetAllChildren())
        {
            // Use this because Detroy doesn't work in Editor
            DestroyImmediate(child);
        }

        foreach (GameObject obj in newTile.GetAllChildren())
        {
            Instantiate(obj, transform);
        }
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
    public void UpdateCollider(Collider originalCollider)
    {
        DestroyImmediate(tileCollider);

        if (originalCollider is BoxCollider)
        {
            BoxCollider original = originalCollider.GetComponent<BoxCollider>();
            BoxCollider newCollider = transform.AddComponent<BoxCollider>();

            newCollider.center = original.center;
            newCollider.size = original.size;
        }

        if (originalCollider is MeshCollider)
        {
            MeshCollider original = originalCollider.GetComponent<MeshCollider>();
            MeshCollider newCollider = transform.AddComponent<MeshCollider>();

            newCollider.sharedMesh = original.sharedMesh;
            newCollider.convex = original.convex;
        }
    }
}
