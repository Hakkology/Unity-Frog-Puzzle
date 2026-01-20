public enum TongueInteractionResult
{
    Continue,   // Move to next cell
    Stop,       // Blocked
    EatAndStop, // Eat this and stop (e.g. wrong color grape?) - Wait, wrong color stops.
    EatAndContinue, // Eat and keep going (e.g. correct grape, though game rules might vary)
    Turn        // Changed direction (Arrow)
}

public interface ITongueInteractable
{
    TongueInteractionResult OnTongueEncounter(Frog frog, ref Direction currentDir);
}
