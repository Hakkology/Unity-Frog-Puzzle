using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the core game loop, including moves, win/lose conditions, and frog tracking.
/// </summary>
public class GameManager : MonoBehaviour, ISingleton
{
    private bool isReady = false;
    /// <summary>
    /// Gets a value indicating whether the GameManager is initialized.
    /// </summary>
    public bool IsReady => isReady;

    public int MaxMoves { get; private set; }
    public int CurrentMoves { get; private set; }
    public bool IsGameActive { get; private set; }

    public static event Action<int> OnMovesChanged;
    public static event Action OnLevelComplete;
    public static event Action OnLevelFailed;

    private List<Frog> activeFrogs = new List<Frog>();

    public void Init()
    {
        Debug.Log("GameManager initialized");
        isReady = true;
    }
    
    public void RegisterFrog(Frog frog)
    {
        if (!activeFrogs.Contains(frog))
        {
            activeFrogs.Add(frog);
        }
    }
    
    public void UnregisterFrog(Frog frog)
    {
        if (activeFrogs.Contains(frog))
        {
            activeFrogs.Remove(frog);
        }
    }
    
    public void ClearFrogs()
    {
        activeFrogs.Clear();
    }

    public void StartLevel(int maxMoves)
    {
        // Clear old frogs list if starting a new level from scratch, 
        // though usually LevelManager handles destruction. 
        // It's safer to let Frogs register themselves on Start.
        // We can just reset stats here.
        MaxMoves = maxMoves;
        CurrentMoves = maxMoves;
        IsGameActive = true;
        OnMovesChanged?.Invoke(CurrentMoves);
        Debug.Log($"Level Started. Moves: {CurrentMoves}");
    }

    public bool CanMove()
    {
        return IsGameActive && CurrentMoves > 0;
    }

    public void ConsumeMove()
    {
        if (!IsGameActive) return;

        CurrentMoves--;
        OnMovesChanged?.Invoke(CurrentMoves);

        if (CurrentMoves <= 0)
        {
            CheckLoseCondition();
        }
    }

    public void CheckWinCondition()
    {
        // Check if all frogs are fed (inactive/removed from list or specifically logic-checked)
        // If we remove them from the list when they are fed (Unregister), then Count == 0 is win.
        // But Unregister might happen on Destroy too.
        // Let's assume Register keeps them, and we check their active state.
        
        bool allFed = true;
        foreach (var frog in activeFrogs)
        {
            // If the frog object is active, it means it hasn't been fed/disabled yet.
            if (frog != null && frog.gameObject.activeInHierarchy)
            {
                allFed = false;
                break;
            }
        }

        if (allFed && activeFrogs.Count > 0) // Ensure we actually had frogs
        {
            IsGameActive = false;
            OnLevelComplete?.Invoke();
            Debug.Log("Level Complete!");
        }
    }

    private void CheckLoseCondition()
    {
        bool anyFrogLeft = false;
        foreach (var frog in activeFrogs)
        {
            if (frog != null && frog.gameObject.activeInHierarchy)
            {
                anyFrogLeft = true;
                break;
            }
        }

        if (CurrentMoves <= 0 && anyFrogLeft)
        {
            IsGameActive = false;
            OnLevelFailed?.Invoke();
            Debug.Log("Level Failed! Out of moves.");
        }
    }
}
