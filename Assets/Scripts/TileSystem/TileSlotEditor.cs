using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]

public class TileSlotEditor : Editor
{
    private GUIStyle centeredStyle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        TileSetHolder tileHolder = FindFirstObjectByType<TileSetHolder>();

        float oneButtonWidth = EditorGUIUtility.currentViewWidth - 25;
        float twoButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;
        float threeButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 3;

        GUILayout.Label("Tile Options", centeredStyle);
        GUILayout.BeginHorizontal();
        MakeButtonSwitchTile("Field", tileHolder.tileField, threeButtonWidth);
        MakeButtonSwitchTile("Road", tileHolder.tileRoad, threeButtonWidth);
        MakeButtonSwitchTile("Sideway", tileHolder.tileSideway, threeButtonWidth);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        MakeButtonSwitchTile("Hill 1", tileHolder.tileHill_1, threeButtonWidth);
        MakeButtonSwitchTile("Hill 2", tileHolder.tileHill_2, threeButtonWidth);
        MakeButtonSwitchTile("Hill 3", tileHolder.tileHill_3, threeButtonWidth);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        MakeButtonSwitchTile("Bridge Field", tileHolder.tileBridgeField, threeButtonWidth);
        MakeButtonSwitchTile("Bridge Road", tileHolder.tileBridgeRoad, threeButtonWidth);
        MakeButtonSwitchTile("Bridge Sideway", tileHolder.tileBridgeSideway, threeButtonWidth);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        MakeButtonSwitchTile("Big Inner Corner", tileHolder.tileCornerInnerBig, twoButtonWidth);
        MakeButtonSwitchTile("Small Inner Corner", tileHolder.tileCornerInnerSmall, twoButtonWidth);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        MakeButtonSwitchTile("Big Outer Corner", tileHolder.tileCornerOuterBig, twoButtonWidth);
        MakeButtonSwitchTile("Small Outer Corner", tileHolder.tileCornerOuterSmall, twoButtonWidth);
        GUILayout.EndHorizontal();

        GUILayout.Label("Rotate Options", centeredStyle);
        GUILayout.BeginHorizontal();
        MakeButtonRotateTile("Rotate Left", -1, twoButtonWidth);
        MakeButtonRotateTile("Rotate Right", 1, twoButtonWidth);
        GUILayout.EndHorizontal();


        GUILayout.Label("Position Options", centeredStyle);
        GUILayout.BeginHorizontal();
        MakeButtonPositionTile("Up", 1, twoButtonWidth);
        MakeButtonPositionTile("Down", -1, twoButtonWidth);
        GUILayout.EndHorizontal();
    }

    // Make a simple button that return true when clicked
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

    private void MakeButtonRotateTile(string tileText, int direction, float buttonWidth)
    {
        if (GUILayout.Button(tileText, GUILayout.Width(buttonWidth)))
        {
            foreach (var target in targets)
            {
                ((TileSlot)target).AdjustYRotation(direction);
            }
        }
    }

    private void MakeButtonPositionTile(string tileText, int verticalDirection, float buttonWidth)
    {
        if (GUILayout.Button(tileText, GUILayout.Width(buttonWidth)))
        {
            foreach (var target in targets)
            {
                ((TileSlot)target).AdjustYPosition(verticalDirection);
            }
        }
    }
}