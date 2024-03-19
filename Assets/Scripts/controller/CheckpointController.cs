using mono.player;
using service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace controller
{
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;

        private void OnEnable()
        {
            InputController.Instance.InputMaster.Player.SpawnCheckpoint.performed += TrySpawnCheckpoint;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.Player.SpawnCheckpoint.performed -= TrySpawnCheckpoint;
        }

        private void TrySpawnCheckpoint(InputAction.CallbackContext _)
        {
            if (!_playerMovement.CanSpawnCheckpoint) return;
            CheckpointService.Instance.TrySpawnCheckpoint();
        }
    }
}