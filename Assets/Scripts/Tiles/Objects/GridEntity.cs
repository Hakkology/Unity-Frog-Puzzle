using UnityEngine;

public class GridEntity : BaseObject, IGridEntity
{
    protected ColorSet colorSet;
    protected Tile cell; 
    public Vector2Int boardPosition;
    public override void Interact()
    {

    }

    public void SetCell(Tile cell)
    {
        this.cell = cell;
        this.boardPosition = new Vector2Int(cell.gridX, cell.gridY);
    }

    public virtual void HandleTextureChange(Texture2D objTexture, Texture2D cellTexture, ColorSet color)
    {
        this.colorSet = color;

        if (textureRenderer != null)
            textureRenderer.material.mainTexture = objTexture;
        
        if (cell != null && cell.textureRenderer != null)
            cell.UpdateTexture(cellTexture);
        
    }
    public ColorSet GetColorSet()
    {
        return colorSet;
    }

    public bool IsColorMatch(BaseObject obj)
    {
        if (obj is GridEntity dynObj)
        {
            bool match = dynObj.colorSet == this.colorSet;
            Debug.Log($"Comparing ColorSet: {this.colorSet} with {dynObj.colorSet} - Match: {match}");
            return match;
        }
        Debug.Log("Object is not a GridEntity");
        return false;
    }
}
