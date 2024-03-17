using mono.player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;

        private void OnEnable()
        {
            InputController.Instance.InputMaster.Player.Interact.performed += TryInteract;
            InputController.Instance.InputMaster.Player.Jump.performed += TryJump;
            InputController.Instance.InputMaster.Player.Glide.performed += AttemptGlide;
            InputController.Instance.InputMaster.Player.Glide.canceled += StopGlide;
            InputController.Instance.InputMaster.Player.Sprint.performed += AttemptSprint;
            InputController.Instance.InputMaster.Player.Sprint.canceled += StopSprint;
            InputController.Instance.InputMaster.Player.Move.performed += AttemptMovement;
            InputController.Instance.InputMaster.Player.Move.canceled += StopMovement;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.Player.Interact.performed -= TryInteract;
        }

        private void AttemptMovement(InputAction.CallbackContext context)
        {
            _playerMovement.AttemptMovement(context.ReadValue<Vector2>());
        }

        private void StopMovement(InputAction.CallbackContext _)
        {
            _playerMovement.AttemptMovement(Vector2.zero);
        }

        private void StopSprint(InputAction.CallbackContext _)
        {
            _playerMovement.AttemptSprint(false);
        }

        private void AttemptSprint(InputAction.CallbackContext _)
        {
            _playerMovement.AttemptSprint(true);
        }

        private void StopGlide(InputAction.CallbackContext _)
        {
            _playerMovement.AttemptGlide(false);
        }

        private void AttemptGlide(InputAction.CallbackContext _)
        {
            _playerMovement.AttemptGlide(true);
        }

        private void TryJump(InputAction.CallbackContext _)
        {
            _playerMovement.TryJump();
        }

        private void TryInteract(InputAction.CallbackContext _)
        {
            _playerMovement.TryInteractWithObject();
        }
    }
}