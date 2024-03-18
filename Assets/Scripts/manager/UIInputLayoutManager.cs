using controller;
using mono.input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace manager
{
    public class UIInputLayoutManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> _onInputLayoutChanged;

        private bool _hasBeenSetUp;

        /// <summary>
        ///     True if the mouse layout is used, false if the controlled layout is used.
        /// </summary>
        private bool _mouseLayoutUsed;

        private void OnEnable()
        {
            InputController.Instance.InputMaster.InputLayout.SwitchToControlled.performed += TrySwitchToControlled;
            InputController.Instance.InputMaster.InputLayout.SwitchToMouse.performed += TrySwitchToMouse;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.InputLayout.SwitchToControlled.performed -= TrySwitchToControlled;
            InputController.Instance.InputMaster.InputLayout.SwitchToMouse.performed -= TrySwitchToMouse;
        }

        private void TrySwitchToMouse(InputAction.CallbackContext _)
        {
            if (_hasBeenSetUp && _mouseLayoutUsed) return;
            SwitchToMouseLayout();
        }

        private void TrySwitchToControlled(InputAction.CallbackContext _)
        {
            if (_hasBeenSetUp && !_mouseLayoutUsed) return;
            SwitchToControlledLayout();
        }

        private void SwitchToControlledLayout()
        {
            _mouseLayoutUsed = false;
            _hasBeenSetUp = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(UISection.Current.SelectedStartObjectOnControlledInput);

            _onInputLayoutChanged.Invoke(false);
        }

        private void SwitchToMouseLayout()
        {
            _mouseLayoutUsed = true;
            _hasBeenSetUp = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);

            _onInputLayoutChanged.Invoke(true);
        }
    }
}