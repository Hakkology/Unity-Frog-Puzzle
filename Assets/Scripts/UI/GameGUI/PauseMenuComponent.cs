public class PauseMenuComponent: GameMenuComponent {
    public void OnContinueButtonClicked() =>  controller.SetPlayMode();
    public void OnSettingsButtonClicked() => controller.OpenSettingsMenu();
    public void OnExitButtonClicked() => controller.GoToMainMenu();
}
