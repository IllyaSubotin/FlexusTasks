public class GameModeService
{
    public GameMode Mode { get; private set; }

    public void SetMode(GameMode mode)
    {
        Mode = mode;
    }

    public bool IsOnline => Mode == GameMode.Online;
    public bool IsOffline => Mode == GameMode.Offline;
}

public enum GameMode
{
    Offline,
    Online,
}
