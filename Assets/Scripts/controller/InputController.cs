using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace controller
{
    public class InputController : MonoBehaviour
    {
        public enum ControlType
        {
            KEYBOARD_MOUSE,
            DUAL_SHOCK_GAMEPAD,
            OTHER_GAMEPAD
        }

        public ControlType CurrentControlType { private set; get; }

        /// <summary>
        ///     Provides access to the input master, allowing scripts to subscribe to input events
        /// </summary>
        public InputMaster InputMaster { private set; get; }

        /// <summary>
        ///     The singleton instance
        /// </summary>
        public static InputController Instance { get; private set; }

        /// <summary>
        ///     Creates the singleton instance and manages the DDOL object
        /// </summary>
        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            InputMaster = new();
            InputMaster.Enable();
        }

        private void OnEnable()
        {
            InputMaster.Enable();

            Instance.InputMaster.InputLayout.AnyMouseOrKey.performed += SwitchToMouse;
            Instance.InputMaster.InputLayout.AnyGamepad.performed += SwitchToGamepad;
        }

        private void OnDisable()
        {
            InputMaster.Disable();
        }

        private void SwitchToGamepad(InputAction.CallbackContext _)
        {
            if (Gamepad.current is DualShockGamepad) CurrentControlType = ControlType.DUAL_SHOCK_GAMEPAD;
            else CurrentControlType = ControlType.OTHER_GAMEPAD;
        }

        private void SwitchToMouse(InputAction.CallbackContext _)
        {
            CurrentControlType = ControlType.KEYBOARD_MOUSE;
        }
    }
}