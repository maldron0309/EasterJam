using UnityEngine;

namespace controller
{
    public class InputController : MonoBehaviour
    {
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
        }

        private void OnDisable()
        {
            InputMaster.Disable();
        }
    }
}