using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BootstrapLevels : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Generate Default Levels")]
    public static void GenerateLevels()
    {
        // Level 1: Simple Line
        // F(R) -> G(R)
        CreateLevel("Level_1", (config) => {
            AddObject(config, 0, 0, "Frog", ColorSet.Red, Direction.Right);
            AddObject(config, 2, 0, "Grape", ColorSet.Red, Direction.Up);
        });

        // Level 2: Turn
        // F(B) -> A(B, Up) -> G(B)
        CreateLevel("Level_2", (config) => {
             AddObject(config, 0, 0, "Frog", ColorSet.Blue, Direction.Right);
             AddObject(config, 2, 0, "Arrow", ColorSet.Blue, Direction.Up);
             AddObject(config, 2, 2, "Grape", ColorSet.Blue, Direction.Up);
        });
        
        // Level 3: Interaction
        // F1(G) -> G(G)
        // F2(Y) -> G(Y) [blocked by F1 initially?] No, parallel
        CreateLevel("Level_3", (config) => {
             AddObject(config, 0, 0, "Frog", ColorSet.Green, Direction.Up);
             AddObject(config, 0, 3, "Grape", ColorSet.Green, Direction.Up);
             
             AddObject(config, 1, 0, "Frog", ColorSet.Yellow, Direction.Up);
             AddObject(config, 1, 4, "Grape", ColorSet.Yellow, Direction.Up);
        });
        
        Debug.Log("Levels Generated in Assets/Resources/Function/Levels/");
    }

    private static void CreateLevel(string name, System.Action<LevelConfig> populate)
    {
        LevelConfig config = ScriptableObject.CreateInstance<LevelConfig>();
        populate(config);
        
        string path = $"Assets/Resources/Function/Levels/{name}.asset";
        
        // Ensure directory exists
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Function"))
             AssetDatabase.CreateFolder("Assets/Resources", "Function");
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Function/Levels"))
             AssetDatabase.CreateFolder("Assets/Resources/Function", "Levels");
             
        AssetDatabase.CreateAsset(config, path);
        EditorUtility.SetDirty(config);
    }

    private static void AddObject(LevelConfig config, int x, int y, string type, ColorSet color, Direction dir)
    {
        var tile = config.GetTileConfig(x, y);
        if (tile == null)
        {
            tile = new LevelConfig.TileConfig { x = x, y = y };
            config.tiles.Add(tile);
        }
        
        tile.objects.Add(new LevelConfig.ObjectConfig {
            objectType = type,
            color = color,
            direction = dir,
            verticalPosition = 0
        });
    }
#endif
}
