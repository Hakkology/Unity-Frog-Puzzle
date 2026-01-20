using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class representing an object that can be placed on a tile and has a direction.
/// </summary>
public abstract class DirectionalEntity : GridEntity, IDirectionalEntity
{
    public Direction facingDirection;

    public void Init() 
    {
        DetermineFacingDirection();
        RotateObjectToFaceDirection();
    }

    private void DetermineFacingDirection()
    {
        var possibleDirections = new List<Direction>();

        if (boardPosition.y < 6 - 1) // Üst sınır kontrolü
            possibleDirections.Add(Direction.Up);
        if (boardPosition.y > 0) // Alt sınır kontrolü
            possibleDirections.Add(Direction.Down);
        if (boardPosition.x < 6 - 1) // Sağ sınır kontrolü
            possibleDirections.Add(Direction.Right);
        if (boardPosition.x > 0) // Sol sınır kontrolü
            possibleDirections.Add(Direction.Left);

        if (possibleDirections.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleDirections.Count);
            facingDirection = possibleDirections[randomIndex];
            Debug.Log($"Determined facing direction for {this.name}: {facingDirection} at {boardPosition.x}, {boardPosition.y})");
        }
        else
        {
            Debug.LogError("No valid directions available.");
        }
    }

    public virtual void RotateObjectToFaceDirection()
    {
        Debug.Log($"Rotating to face direction: {facingDirection}");
        float angle = facingDirection switch
        {
            Direction.Up => 0,
            Direction.Right => 90,
            Direction.Down => 180,
            Direction.Left => 270,
            _ => 0,
        };

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    protected Vector2Int DirectionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector2Int(0, 1);
            case Direction.Down:
                return new Vector2Int(0, -1);
            case Direction.Left:
                return new Vector2Int(-1, 0);
            case Direction.Right:
                return new Vector2Int(1, 0);
            default:
                throw new System.ArgumentOutOfRangeException("Unknown direction");
        }
    }
}
