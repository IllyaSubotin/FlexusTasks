using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MenuState : State
{
    private MenuScreen screen;
    private GameModeService _gameModeService;

    [Inject]
    private void Construct(MenuScreen menuScreen, GameModeService gameModeService)
    {
        screen = menuScreen;
        _gameModeService = gameModeService;
    }


    public override void Enter()
    {
        screen.playOfflineButton.onClick.AddListener(() =>
        {
            _gameModeService.SetMode(GameMode.Offline);
            SceneManager.LoadScene("GameplayScene");
        });

        screen.playOnlineButton.onClick.AddListener(() =>
        {
            _gameModeService.SetMode(GameMode.Online);
            SceneManager.LoadScene("GameplayScene");
        });

        screen.exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public override void Exit()
    {
        screen.playOfflineButton.onClick.RemoveAllListeners();
        screen.playOnlineButton.onClick.RemoveAllListeners();
        screen.exitButton.onClick.RemoveAllListeners();
    }
}
