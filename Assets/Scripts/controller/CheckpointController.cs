using service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace controller
{
    public class CheckpointController : MonoBehaviour
    {
        private void OnEnable()
        {
            InputController.Instance.InputMaster.Player.SpawnCheckpoint.performed += TrySpawnCheckpoint;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.Player.SpawnCheckpoint.performed -= TrySpawnCheckpoint;
        }

        private static void TrySpawnCheckpoint(InputAction.CallbackContext _)
        {
            CheckpointService.Instance.TrySpawnCheckpoint();
        }
    }
}