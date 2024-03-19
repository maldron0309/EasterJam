using controller;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UI;

namespace mono.ui
{
    public class InputSprite : MonoBehaviour
    {
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Sprite _dualshockGamepadSprite;
        [SerializeField] private Sprite _otherGamepadSprite;

        [SerializeField] private Image _image;
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private bool _hasBeenSetUp;
        private bool _usesGamepadInput;

        private void OnEnable()
        {
            InputController.Instance.InputMaster.InputLayout.AnyMouseOrKey.performed += SwitchToMouse;
            InputController.Instance.InputMaster.InputLayout.AnyGamepad.performed += SwitchToGamepad;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.InputLayout.AnyMouseOrKey.performed -= SwitchToMouse;
            InputController.Instance.InputMaster.InputLayout.AnyGamepad.performed -= SwitchToGamepad;
        }

        private void SwitchToGamepad(InputAction.CallbackContext _)
        {
            if (Gamepad.current == null || (_usesGamepadInput && _hasBeenSetUp)) return;

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