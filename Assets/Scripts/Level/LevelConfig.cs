using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewLevelLayout", menuName = "Level/Level Layout")]
public class LevelLayout : ScriptableObject
{
    public string levelName;
    public Sprite levelImage;
    public LevelConfig levelConfig;
}

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Level Configuration/New Level Configuration")]
public class LevelConfig : ScriptableObject
{
    public List<TileConfig> tiles = new List<TileConfig>();
    public TileConfig GetTileConfig(int x, int y)
    {
        return tiles.Find(t => t.x == x && t.y == y);
    }

    [System.Serializable]
    public class TileConfig
    {
        public int x, y;
        public List<ObjectConfig> objects = new List<ObjectConfig>();
    }

    [System.Serializable]
    public class ObjectConfig
    {
        public string objectType;
        public Direction direction;
        public ColorSet color;
        public float verticalPosition;
    }
}
