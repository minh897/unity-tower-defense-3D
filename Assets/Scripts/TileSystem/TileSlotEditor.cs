using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]

public class TileSlotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        float buttonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;

        GUILayout.BeginHorizontal();

        // Make a simple that return true when clicked
        // Find TileSetHolder component and apply a new tile
        // Switch each selected tile to the new tile
        MakeButtonSwitchTile("Field", FindFirstObjectByType<TileSetHolder>().tileField, buttonWidth);
        MakeButtonSwitchTile("Road", FindFirstObjectByType<TileSetHolder>().tileRoad, buttonWidth);

        // if (GUILayout.Button("Road", GUILayout.Width(buttonWidth)))
        // {
        //     GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileRoad;
        //     foreach (var target in targets)
        //     {
        //         ((TileSlot)target).SwitchTile(newTile);
        //     }
        // }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        MakeButtonSwitchTile("Sideway", FindFirstObjectByType<TileSetHolder>().tileSideway, buttonWidth * 2);
        
        // if (GUILayout.Button("Sideway", GUILayout.Width(buttonWidth * 2)))
        // {
        //     GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileSideway;
        //     foreach (var target in targets)
        //     {
        //         ((TileSlot)target).SwitchTile(newTile);
        //     }
        // }

        GUILayout.EndHorizontal();
    }

    private void MakeButtonSwitchTile(string tileText, GameObject tileObject, float buttonWidth)
    {
        if (GUILayout.Button(tileText, GUILayout.Width(buttonWidth)))
        {
            foreach (var target in targets)
            {
                ((TileSlot)target).SwitchTile(tileObject);
            }
        }
    }
}
