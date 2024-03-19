using controller;
using mono.input;
using mono.ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

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
            InputController.Instance.InputMaster.InputLayout.Confirm.performed += TryConfirmation;
            InputController.Instance.InputMaster.InputLayout.Abort.performed += TryAbortion;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.InputLayout.SwitchToControlled.performed -= TrySwitchToControlled;
            InputController.Instance.InputMaster.InputLayout.SwitchToMouse.performed -= TrySwitchToMouse;
            InputController.Instance.InputMaster.InputLayout.Confirm.performed -= TryConfirmation;
            InputController.Instance.InputMaster.InputLayout.Abort.performed -= TryAbortion;
        }

        private void TryAbortion(InputAction.CallbackContext _)
        {
            if (UISection.Current != null)
                UISection.Current.TryAbortion();
        }

        private void TryConfirmation(InputAction.CallbackContext _)
        {
            if (EventSystem.current == null || _mouseLayoutUsed) return;
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;

            if (EventSystem.current.currentSelectedGameObject == null && UISection.Current != null)
                UISection.Current.TryConfirmation();
            else if (EventSystem.current.currentSelectedGameObject != null &&
                     selectedGameObject.TryGetComponent<SelectableButton>(out var selectableButton))
                selectableButton.Execute();
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

        public static void SetGamepadColour(Color color)
        {
            if (Gamepad.current == null || Gamepad.all[0] is not DualShockGamepad dualShockGamepad) return;
            dualShockGamepad.SetLightBarColor(color);
        }

        private void SwitchToControlledLayout()
        {
            _mouseLayoutUsed = false;
            _hasBeenSetUp = true;

            SetGamepadColour(Color.cyan);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (EventSystem.current != null && UISection.Current != null)
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