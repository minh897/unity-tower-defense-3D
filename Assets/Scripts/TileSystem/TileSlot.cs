using System.Collections.Generic;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter meshFilter => GetComponent<MeshFilter>();

    public Material GetMaterial() => meshRenderer.sharedMaterial;

    public Mesh GetMesh() => meshFilter.sharedMesh;

    public void SwitchTile(GameObject referenceTile)
    {
        TileSlot newTile = referenceTile.GetComponent<TileSlot>();

        meshRenderer.material = newTile.GetMaterial();
        meshFilter.mesh = newTile.GetMesh();

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
}
