using UnityEngine;
using DG.Tweening;

/// <summary>
/// Represents an arrow on a tile.
/// </summary>
public class Arrow : DirectionalEntity
{
    public override TongueInteractionResult OnTongueEncounter(Frog frog, ref Direction currentDir)
    {
        // Use public getter for frog's color
        if (this.GetColorSet() == frog.GetColorSet())
        {
            currentDir = this.facingDirection;
            return TongueInteractionResult.Turn;
        }
        return TongueInteractionResult.Stop;
    }
}
