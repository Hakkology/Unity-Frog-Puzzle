using TMPro;
using UnityEngine;

public class GameHUDScoreCounter : MonoBehaviour {
    public TextMeshProUGUI scoreCountText;
    protected GameHUDController controller;
    public void SetupController(GameHUDController gameMenuController) => controller = gameMenuController;
    public void IncrementScore()
    {
        controller.score++;
        UpdateScoreText();
    }
    public void UpdateScoreText() => scoreCountText.text = "Score: " + controller.score.ToString();
}
