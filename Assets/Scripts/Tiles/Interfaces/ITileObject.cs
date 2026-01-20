using UnityEngine;

/// <summary>
/// Defines the basic functionalities for a tile object on the map.
/// </summary>
public interface ITileObject
{
    /// <summary>
    /// Handles the interaction with the tile object.
    /// </summary>
    void Interact();

    /// <summary>
    /// Register Coordinates of the tile.
    /// </summary>
    public void Initialize(int x, int y);

    /// <summary>
    /// Adds a new object to the tile.
    /// </summary>
    /// <param name="tileObject">The object to add.</param>
    void AddTileObject(BaseObject tileObject);

    /// <summary>
    /// Gets the topmost object on the tile.
    /// </summary>
    /// <returns>The topmost object.</returns>
    BaseObject GetTopmostObject();

    /// <summary>
    /// Removes the topmost object from the tile.
    /// </summary>
    void RemoveTopmostObject();
}
