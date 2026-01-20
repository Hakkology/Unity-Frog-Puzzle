using UnityEngine;

/// <summary>
/// Abstract class representing an object that can be placed on a tile.
/// </summary>
public abstract class BaseObject : MonoBehaviour, IBaseObject, ITongueInteractable
{
    public virtual TongueInteractionResult OnTongueEncounter(Frog frog, ref Direction currentDir)
    {
        return TongueInteractionResult.Stop; // Default behavior is obstacle
    }

    public Renderer textureRenderer;
    protected TextureManager textureManager;
    protected TileManager tileManager;
    protected virtual void Awake() 
    {
        textureManager = SingletonManager.GetSingleton<TextureManager>();
        tileManager = SingletonManager.GetSingleton<TileManager>();
    } 
    public virtual void UpdateTexture(Texture2D newTexture) => textureRenderer.material.mainTexture = newTexture;
    public abstract void Interact();
    
}
