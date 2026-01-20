using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction { Up, Down, Left, Right }

/// <summary>
/// The <c>TileManager</c> class is responsible for managing all the tiles in the game.
/// It uses a dictionary to map tile positions to their respective tile objects for efficient access.
/// </summary>
public class TileManager : MonoBehaviour, ISingleton
{
    /// <summary>
    /// Event triggered when the TileManager is ready.
    /// </summary>
    public static event Action OnTileManagerReady;

    /// <summary>
    /// Dictionary to store tiles with their positions as keys.
    /// </summary>
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    /// <summary>
    /// Indicates whether the TileManager is ready.
    /// </summary>
    private bool isReady = false;

    /// <summary>
    /// Gets a value indicating whether the TileManager is ready.
    /// </summary>
    public bool IsReady => isReady;
    
    /// <summary>
    /// Initializes the TileManager and triggers the OnTileManagerReady event.
    /// </summary>
    public void Init() 
    {
        isReady = true;
        OnTileManagerReady?.Invoke();
    }
    
    /// <summary>
    /// Registers event handlers for scene loading.
    /// </summary>
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    /// <summary>
    /// Unregisters event handlers for scene loading.
    /// </summary>
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    
    /// <summary>
    /// Clears the tiles and triggers the OnTileManagerReady event when a new scene is loaded.
    /// </summary>
    /// <param name="scene">The loaded scene.</param>
    /// <param name="mode">The load mode.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        ClearTiles();
        OnTileManagerReady?.Invoke();
    } 
    
    /// <summary>
    /// Clears the dictionary of tiles.
    /// </summary>
    public void ClearTiles() => tiles.Clear();
    
    /// <summary>
    /// Registers a tile in the dictionary with its position as the key.
    /// </summary>
    /// <param name="tile">The tile to register.</param>
    public void RegisterTile(Tile tile)
    {
        Vector2Int position = new Vector2Int(tile.gridX, tile.gridY);
        if (!tiles.ContainsKey(position))
        {
            tiles[position] = tile;
            Debug.Log($"Tile registered at position ({tile.gridX}, {tile.gridY})");
        }
    }

    /// <summary>
    /// Retrieves a tile at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the tile position.</param>
    /// <param name="y">The y-coordinate of the tile position.</param>
    /// <returns>The tile at the specified position, or null if no tile is found.</returns>
    public Tile GetTileAt(int x, int y)
    {
        Vector2Int position = new Vector2Int(x, y);
        if (tiles.TryGetValue(position, out Tile tile))
        {
            Debug.Log($"Tile found at position ({x}, {y})");
            return tile;
        }
        Debug.LogError($"No tile found at position ({x}, {y})");
        return null;
    }

    /// <summary>
    /// Rotates an object on the specified tile to face the given direction.
    /// </summary>
    /// <param name="x">The x-coordinate of the tile position.</param>
    /// <param name="y">The y-coordinate of the tile position.</param>
    /// <param name="direction">The direction to face.</param>
    public void SetRotation(int x, int y, Direction direction)
    {
        Tile tile = GetTileAt(x, y);
        if (tile != null)
        {
            DirectionalEntity directionalEntity = tile.GetTopmostObject() as DirectionalEntity;
            if (directionalEntity != null)
            {
                directionalEntity.facingDirection = direction;
                directionalEntity.RotateObjectToFaceDirection();
            }
        }
    }
}
