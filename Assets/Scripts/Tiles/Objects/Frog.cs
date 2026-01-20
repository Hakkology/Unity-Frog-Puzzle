using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Represents a Frog entity on the grid, capable of extending its tongue to interact with other objects.
/// </summary>
public class Frog : DirectionalEntity
{
    public Transform tongueBone;
    public TongueController tongue;
    private bool isAnimating = false;
    
    protected override void Awake() 
    {
        base.Awake();
        Debug.Log("Frog Awake called, subscribing to Texture Change.");
        textureRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        tongue = GetComponentInChildren<TongueController>();
    } 

    private void Start() 
    {
        var textures = textureManager.GetRandomFrogTexture();
        HandleTextureChange(textures.frogTexture, textures.cellTexture, textures.color);
        
        // Register with GameManager
        SingletonManager.GetSingleton<GameManager>()?.RegisterFrog(this);
    }
    
    private void OnDestroy()
    {
        SingletonManager.GetSingleton<GameManager>()?.UnregisterFrog(this);
    }

    private List<Vector2Int> currentTravelPath;

    public override void Interact()
    {
        GameManager gameManager = SingletonManager.GetSingleton<GameManager>();
        
        if (!isAnimating && gameManager != null && gameManager.CanMove())
        {
            isAnimating = true;
            currentTravelPath = GetPath();
            
            if (currentTravelPath.Count > 0)
            {
                Debug.Log("Starting tongue animation.");
                gameManager.ConsumeMove();
                
                tongue.StartExtendTongue(currentTravelPath);
                
                // Subscribe to retraction
                tongue.OnTongueRetracted += OnTongueRetractedHandler;
            }
            else
            {
                Debug.Log("Invalid move. Shaking.");
                transform.DOShakePosition(0.5f, 0.5f, 10, 90f).OnComplete(() => isAnimating = false);
            }
        }
    }

    private void OnTongueRetractedHandler()
    {
        tongue.OnTongueRetracted -= OnTongueRetractedHandler; // Unsubscribe
        
        // Handle outcome
        HandleEating(currentTravelPath);
        
        isAnimating = false;
        SingletonManager.GetSingleton<GameManager>()?.CheckWinCondition();
        currentTravelPath = null;
    }

    private void HandleEating(List<Vector2Int> path)
    {
        if (path == null || path.Count == 0) return;

        if (path == null || path.Count == 0) return;

        // Check if the final state is a 'Success' state.
        // For now, Success is defined as landing on a matching Grape.
        Vector2Int lastPos = path[path.Count - 1];
        Tile lastTile = tileManager.GetTileAt(lastPos.x, lastPos.y);
        BaseObject lastObj = lastTile.GetTopmostObject();

        // Check if the final state is a 'Success' state.
        // For now, Success is defined as landing on a matching Grape.
        if (lastObj is Grape g && IsColorMatch(g))
        {
             // Confirm eating logic
             foreach(var pos in path)
             {
                 Tile t = tileManager.GetTileAt(pos.x, pos.y);
                 BaseObject o = t.GetTopmostObject();
                 if (o is Grape grape && IsColorMatch(grape))
                 {
                     t.RemoveTopmostObject();
                 }
             }
             this.gameObject.SetActive(false);
             Debug.Log("Frog fed and deactivated.");
        }
        else
        {
             Debug.Log("Tongue retracted empty-handed.");
        }
    }

    private List<Vector2Int> GetPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentPosition = new Vector2Int(cell.gridX, cell.gridY);
        Direction currentDirection = facingDirection;

        int maxSteps = 100;
        int steps = 0;

        if (tileManager == null) 
            tileManager = SingletonManager.GetSingleton<TileManager>();

        while (steps < maxSteps)
        {
            steps++;
            Vector2Int nextPos = currentPosition + DirectionToVector(currentDirection);
            Tile nextTile = tileManager.GetTileAt(nextPos.x, nextPos.y);

            if (nextTile == null) break; // Boundary

            currentPosition = nextPos;
            path.Add(currentPosition);

            BaseObject topmostObject = nextTile.GetTopmostObject();
            if (topmostObject == null) continue; // Empty cell

            // Polymorphic Interaction
            ITongueInteractable interactable = topmostObject as ITongueInteractable;
            if (interactable != null)
            {
                TongueInteractionResult result = interactable.OnTongueEncounter(this, ref currentDirection);
                
                if (result == TongueInteractionResult.Stop)
                {
                    Debug.Log($"Blocked by {topmostObject.name}");
                    break;
                }
                else if (result == TongueInteractionResult.Turn)
                {
                    Debug.Log($"Turned by {topmostObject.name}");
                    // Direction updated by ref
                }
                else if (result == TongueInteractionResult.EatAndContinue)
                {
                    Debug.Log($"Passed matching {topmostObject.name}");
                    // Continue loop
                }
                // If we defined EatAndStop, we would handle it here.
            }
            else
            {
                // Non-interactable object (should be Stop by default logic or BaseObject implementation)
                Debug.Log($"Blocked by unknown {topmostObject.name}");
                break;
            }
        }
        return path;
    }
}
