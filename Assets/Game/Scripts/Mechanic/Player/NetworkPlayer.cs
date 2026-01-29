using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private PlayerMovementController _playerController;

    private void Start()
    {
        if (!IsOwner)
        {
            _playerController.BlockMovement(true);
        }
    }
}
