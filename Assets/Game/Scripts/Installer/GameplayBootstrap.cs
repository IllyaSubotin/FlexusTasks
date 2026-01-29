using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class GameplayBootstrap : MonoBehaviour
{
    [Header("Offline Player Prefab")]
    [SerializeField] private GameObject _prefab;

    [Header("Camera")]
    [SerializeField] private Camera _cameraPlayer;
    [SerializeField] private PlayerCameraController _playerCameraController;

    private GameModeService _gameModeService;
    
    [Inject]
    private void Construct(GameModeService gameModeService)
    {
        _gameModeService = gameModeService;
    }

    private async void Start()
    {
        if (_gameModeService.IsOffline)
        {
            StartOffline();
            Debug.Log("OFFLINE STARTED");
        }
        else
        {
            await StartOnlineAsync();
            Debug.Log("ONLINE STARTED");
        }
    }

    // ---------------- OFFLINE ----------------

    private void StartOffline()
    {
        SpawnOfflinePlayer();
    }

    private void SpawnOfflinePlayer()
    {
        var player = Instantiate(_prefab, Vector3.zero, Quaternion.identity);

        player.GetComponent<PlayerMovementController>().SetCamera(_cameraPlayer, _playerCameraController);
    }

    // ---------------- ONLINE ----------------

    private async Task StartOnlineAsync()
    {
        if (!TryStartHost())
            StartClient();
        

        await InitOnlinePlayerAsync();
    }

    private bool TryStartHost()
    {
        bool success = NetworkManager.Singleton.StartHost();

        if (success)
            Debug.Log("HOST STARTED");

        return success;
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("CLIENT STARTED");
    }

    private async Task InitOnlinePlayerAsync()
    {
        NetworkObject player = null;

        while (player == null)
        {
            player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            await Task.Yield();
        }

        player.GetComponent<PlayerMovementController>().SetCamera(_cameraPlayer, _playerCameraController);

        Debug.Log("ONLINE PLAYER INITIALIZED");
    }
}
