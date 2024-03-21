using controller;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UI;

#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace mono.ui
{
    public class InputSprite : MonoBehaviour
    {
        [Header("Sprite Types")] [SerializeField]
        private Sprite _emptySprite;

        [SerializeField] private Sprite _dualshockGamepadSprite;
        [SerializeField] private Sprite _otherGamepadSprite;

        [Header("Control Types")] [SerializeField]
        private bool _controlsLayout;

        [SerializeField] private bool _affectsImage;
        [SerializeField] private bool _affectsSpriteRenderer;
        [SerializeField] private bool _reactToPriorInput;

        [ShowIf("_controlsLayout")] [SerializeField]
        private LayoutElement _layoutElement;

        [ShowIf("_affectsImage")] [SerializeField]
        private Image _image;

        [ShowIf("_affectsSpriteRenderer")] [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private bool _hasBeenSetUp;
        private bool _usesGamepadInput;

        private void OnEnable()
        {
            InputController.Instance.InputMaster.InputLayout.AnyMouseOrKey.performed += SwitchToMouse;
            InputController.Instance.InputMaster.InputLayout.AnyGamepad.performed += SwitchToGamepad;

            if (!_reactToPriorInput) return;
            if (InputController.Instance.CurrentControlType is InputController.ControlType.KEYBOARD_MOUSE)
                ApplyMouse();
            else ApplyGamepad();
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.InputLayout.AnyMouseOrKey.performed -= SwitchToMouse;
            InputController.Instance.InputMaster.InputLayout.AnyGamepad.performed -= SwitchToGamepad;
        }

        private void SwitchToGamepad(InputAction.CallbackContext _)
        {
            if (Gamepad.current == null || (_usesGamepadInput && _hasBeenSetUp)) return;
            ApplyGamepad();
        }

        private void ApplyGamepad()
        {
            _usesGamepadInput = true;
            _hasBeenSetUp = true;

            if (_spriteRenderer != null)
                _spriteRenderer.sprite =
                    Gamepad.current is DualShockGamepad ? _dualshockGamepadSprite : _otherGamepadSprite;

            if (_image == null) return;
            _image.sprite = Gamepad.current is DualShockGamepad ? _dualshockGamepadSprite : _otherGamepadSprite;

            if (_layoutElement == null) return;
            _layoutElement.ignoreLayout = false;
        }

        private void SwitchToMouse(InputAction.CallbackContext _)
        {
            if (!_usesGamepadInput && _hasBeenSetUp) return;
            ApplyMouse();
        }

        private void ApplyMouse()
        {
            _usesGamepadInput = false;
            _hasBeenSetUp = true;

            if (_spriteRenderer != null) _spriteRenderer.sprite = _emptySprite;

            if (_image == null) return;

            _image.sprite = _emptySprite;

            if (_layoutElement == null) return;
            _layoutElement.ignoreLayout = true;
        }
    }
}