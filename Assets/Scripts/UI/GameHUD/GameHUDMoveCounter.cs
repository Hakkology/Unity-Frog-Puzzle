
using TMPro;
using UnityEngine;

public class GameHUDMoveCounter : MonoBehaviour {
    public TextMeshProUGUI moveCountText;
    private GameHUDController controller;

    public void SetupController(GameHUDController gameMenuController) => controller = gameMenuController;
    public void UpdateMovesText() => moveCountText.text = "Moves: " + controller.moves.ToString();
    
}
