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

        // Field and road
        GUILayout.BeginHorizontal();

        MakeButtonSwitchTile("Field", FindFirstObjectByType<TileSetHolder>().tileField, buttonWidth);
        MakeButtonSwitchTile("Road", FindFirstObjectByType<TileSetHolder>().tileRoad, buttonWidth);

        GUILayout.EndHorizontal();

        // Sideway
        GUILayout.BeginHorizontal();

        MakeButtonSwitchTile("Sideway", FindFirstObjectByType<TileSetHolder>().tileSideway, buttonWidth * 2);

        GUILayout.EndHorizontal();

        // Corners
        GUILayout.BeginHorizontal();

        MakeButtonSwitchTile("Inner Corner", FindFirstObjectByType<TileSetHolder>().tileInnerCorner, buttonWidth);
        MakeButtonSwitchTile("Outer Corner", FindFirstObjectByType<TileSetHolder>().tileOuterCorner, buttonWidth);

        GUILayout.EndHorizontal();
    }

    // Make a simple that return true when clicked
    // Find TileSetHolder component and apply a new tile
    // Switch each selected tile to the new tile
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
