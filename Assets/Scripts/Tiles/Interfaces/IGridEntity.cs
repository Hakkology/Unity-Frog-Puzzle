using UnityEngine;

public interface IGridEntity
{
    /// <summary>
    /// Sets the cell renderer for the object.
    /// </summary>
    /// <param name="cellRenderer">The renderer of the cell on which the object resides.</param>
    void SetCell(Tile cell);
}
