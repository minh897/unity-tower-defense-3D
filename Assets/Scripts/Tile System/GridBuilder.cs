using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private int gridLength = 10;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private GameObject mainPrefab;
    [SerializeField] private List<GameObject> createdTiles;

    private NavMeshSurface myNavMesh => GetComponent<NavMeshSurface>();

    private bool hadFirstLoad;

    public void DisableTileShadow()
    {
        foreach (var tile in createdTiles)
        {
            tile.GetComponent<TileSlot>().DisableShadowIfNeeded();
        }
    }

    public bool IsOnFirstLoad()
    {
        if (hadFirstLoad == false)
        {
            hadFirstLoad = true;
            return true;
        }

        return false;
    }

    private void CreateTile(float xPosition, float zPosition)
    {
        Vector3 newPosition = new(xPosition, 0, zPosition);
        GameObject newTile = Instantiate(mainPrefab, newPosition, Quaternion.identity, transform);
        createdTiles.Add(newTile);

        newTile.GetComponent<TileSlot>().TurnIntoBuildSlot(mainPrefab);
    }

    [ContextMenu("Build Grid")]
    private void BuildGrid()
    {
        // Make sure you cannot create more tiles on top of existing tiles
        ClearGrid();

        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                CreateTile(x, z);
            }
        }

        // DisableTileShadow();
    }

    [ContextMenu("Clear Grid")]
    private void ClearGrid()
    {
        foreach (GameObject tile in createdTiles)
        {
            DestroyImmediate(tile);
        }

        createdTiles.Clear();
    }

    public List<GameObject> GetTileSetup() => createdTiles;

    public void UpdateNewNavMesh() => myNavMesh.BuildNavMesh();

    public void MakeTileNonInteractable(bool isNonInteractable)
    {
        foreach (var tile in createdTiles)
        {
            tile.GetComponent<TileSlot>().MakeNonInteractable(isNonInteractable);
        }
    }
}
