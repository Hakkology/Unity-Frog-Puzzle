using UnityEngine;

public class EntryMenu : MainMenuComponent
{
    public void OnPlayButtonClicked() =>  controller.GoToLevelSelect();
    public void OnSettingsButtonClicked() => controller.GoToSettings();
    public void OnExitButtonClicked() => Application.Quit();
}
