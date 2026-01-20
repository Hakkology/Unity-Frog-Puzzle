using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    private LevelConfig currentLevelConfig;
    private string newLevelName = "NewLevel";
    
    // Selection state
    private string selectedObjectType = "Cell"; // Cell, Frog, Grape, Arrow
    private Direction selectedDirection = Direction.Up;
    private ColorSet selectedColor = ColorSet.Red; // Assuming ColorSet enum exists: Red, Blue, Green, Yellow

    [MenuItem("Window/Frog Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Configuration", EditorStyles.boldLabel);
        
        currentLevelConfig = (LevelConfig)EditorGUILayout.ObjectField("Level Config", currentLevelConfig, typeof(LevelConfig), false);

        if (GUILayout.Button("Create New Level Config"))
        {
            CreateNewLevelConfig();
        }

        if (currentLevelConfig == null) return;

        GUILayout.Space(10);
        GUILayout.Label("Painting Tools", EditorStyles.boldLabel);
        
        selectedObjectType = EditorGUILayout.Popup("Object Type", GetObjectTypeIndex(selectedObjectType), new string[] { "Cell", "Frog", "Grape", "Arrow" }) == 0 ? "Cell" : 
                             (GetObjectTypeIndex(selectedObjectType) == 1 ? "Frog" : 
                             (GetObjectTypeIndex(selectedObjectType) == 2 ? "Grape" : "Arrow"));

        if (selectedObjectType != "Cell")
        {
            selectedColor = (ColorSet)EditorGUILayout.EnumPopup("Color", selectedColor);
            
            if (selectedObjectType == "Frog" || selectedObjectType == "Arrow")
            {
                selectedDirection = (Direction)EditorGUILayout.EnumPopup("Direction", selectedDirection);
            }
        }

        GUILayout.Space(10);
        DrawGrid();
        
        if (GUILayout.Button("Save Changes"))
        {
            EditorUtility.SetDirty(currentLevelConfig);
            AssetDatabase.SaveAssets();
        }
    }
    
    private int GetObjectTypeIndex(string name)
    {
        switch (name)
        {
            case "Frog": return 1;
            case "Grape": return 2;
            case "Arrow": return 3;
            default: return 0;
        }
    }

    private void DrawGrid()
    {
        if (currentLevelConfig == null) return;

        GUILayout.BeginVertical();
        for (int y = 5; y >= 0; y--) // Draw from top (y=5) to bottom (y=0)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 6; x++)
            {
                LevelConfig.TileConfig tile = currentLevelConfig.GetTileConfig(x, y);
                string label = GetTileLabel(tile);
                GUI.backgroundColor = GetTileColor(tile);

                if (GUILayout.Button(label, GUILayout.Width(50), GUILayout.Height(50)))
                {
                    PaintTile(x, y);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
    }

    private string GetTileLabel(LevelConfig.TileConfig tile)
    {
        if (tile == null || tile.objects.Count == 0) return ".";
        var obj = tile.objects[tile.objects.Count - 1]; // Topmost
        string dirInfo = (obj.objectType == "Frog" || obj.objectType == "Arrow") ? obj.direction.ToString().Substring(0, 1) : "";
        return $"{obj.objectType.Substring(0, 1)}{dirInfo}";
    }
    
    private Color GetTileColor(LevelConfig.TileConfig tile)
    {
        if (tile == null || tile.objects.Count == 0) return Color.white;
        var obj = tile.objects[tile.objects.Count - 1];
        switch (obj.color)
        {
            case ColorSet.Red: return Color.red;
            case ColorSet.Blue: return Color.cyan;
            case ColorSet.Green: return Color.green;
            case ColorSet.Yellow: return Color.yellow;
            default: return Color.white;
        }
    }

    private void PaintTile(int x, int y)
    {
        if (currentLevelConfig == null) return;

        LevelConfig.TileConfig tile = currentLevelConfig.GetTileConfig(x, y);
        if (tile == null)
        {
            tile = new LevelConfig.TileConfig { x = x, y = y };
            currentLevelConfig.tiles.Add(tile);
        }

        if (selectedObjectType == "Cell")
        {
            // Clear objects (set to empty cell)
            tile.objects.Clear();
        }
        else
        {
            LevelConfig.ObjectConfig obj = new LevelConfig.ObjectConfig
            {
                objectType = selectedObjectType,
                color = selectedColor,
                direction = selectedDirection,
                verticalPosition = 0 // Basic assumption
            };
            tile.objects.Add(obj);
        }
        
        EditorUtility.SetDirty(currentLevelConfig);
    }

    private void CreateNewLevelConfig()
    {
        LevelConfig asset = ScriptableObject.CreateInstance<LevelConfig>();
        string path = EditorUtility.SaveFilePanelInProject("Save Level Config", newLevelName, "asset", "Please enter a file name to save the level config to");
        if (string.IsNullOrEmpty(path)) return;

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        currentLevelConfig = asset;
    }
}
