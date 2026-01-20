using UnityEngine;

/// <summary>
/// Represents a Grape on a tile.
/// </summary>
public class Grape : GridEntity
{
    public override TongueInteractionResult OnTongueEncounter(Frog frog, ref Direction currentDir)
    {
        if (frog.IsColorMatch(this))
        {
             return TongueInteractionResult.EatAndContinue; 
        }
        return TongueInteractionResult.Stop;
    }
    protected override void Awake() 
    {
        base.Awake();
        textureRenderer = GetComponentInChildren<Renderer>();
    } 

    private void Start() 
    {
        var textures = textureManager.GetRandomGrapeTexture();
        HandleTextureChange(textures.grapeTexture, textures.cellTexture, textures.color);
    }
    
    public override void Interact()
    {

    }
}
