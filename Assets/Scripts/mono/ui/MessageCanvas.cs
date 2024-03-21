using System;
using System.Collections;
using System.Text;
using controller;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace mono.ui
{
    public class MessageCanvas : MonoBehaviour
    {
        public const string CHECKPOINT_SPAWNED_KEY_NAME = "checkpoint-spawned";
        public const string NOT_ENOUGH_CHECKPOINT_MANA_KEY_NAME = "not-enough-checkpoint-mana";

        private const string KEYBOARD_SPRITE_PREFIX = "Keyboard Sprites";
        private const string PLAY_STATION_SPRITE_PREFIX = "PlayStation Sprites";
        private const string OTHER_GAMEPADS_SPRITE_PREFIX = "Other Gamepads Sprites";

        [SerializeField] private LocalizedStringTable _hudMessagesTable;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Image _panelImage;
        private Coroutine _fadeCoroutine;

        private string _fontIconPrefix;

        public static MessageCanvas Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;
        }

        private void OnEnable()
        {
            InputController.Instance.InputMaster.InputLayout.AnyGamepad.performed += TrySwitchToControlled;
            InputController.Instance.InputMaster.InputLayout.AnyMouseOrKey.performed += TrySwitchToMouse;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.InputLayout.AnyGamepad.performed -= TrySwitchToControlled;
            InputController.Instance.InputMaster.InputLayout.AnyMouseOrKey.performed -= TrySwitchToMouse;
        }

        private void TrySwitchToMouse(InputAction.CallbackContext _)
        {
            // Keyboard mouse
            _fontIconPrefix = KEYBOARD_SPRITE_PREFIX;
        }

        private void TrySwitchToControlled(InputAction.CallbackContext _)
        {
            if (_fontIconPrefix is not KEYBOARD_SPRITE_PREFIX) return;

            _fontIconPrefix = OTHER_GAMEPADS_SPRITE_PREFIX;

            // Gamepad dual shock
            if (Gamepad.current is DualShockGamepad)
                _fontIconPrefix = PLAY_STATION_SPRITE_PREFIX;
        }

        public void ShowMessage(LocalizedString message, InputActionReference reference)
        {
            var messageBuilder = new StringBuilder();
            if (reference != null)
            {
                if (_fontIconPrefix == string.Empty)
                    _fontIconPrefix = InputController.Instance.CurrentControlType switch
                    {
                        InputController.ControlType.KEYBOARD_MOUSE => KEYBOARD_SPRITE_PREFIX,
                        InputController.ControlType.DUAL_SHOCK_GAMEPAD => PLAY_STATION_SPRITE_PREFIX,
                        InputController.ControlType.OTHER_GAMEPAD => KEYBOARD_SPRITE_PREFIX,
                        _ => throw new NotImplementedException("Gamepad type not implemented")
                    };

                var iconName =
                    reference.action.actionMap.name.Replace(" ", "_").ToLower() +
                    "-" + reference.action.name.Replace(" ", "_").ToLower();
                var spriteTag = $"<sprite=\"{_fontIconPrefix}\" name=\"{iconName}\">";
                messageBuilder.Append(spriteTag);
            }

            messageBuilder.Append(message.GetLocalizedString());
            _messageText.text = messageBuilder.ToString();
            ShowMessage();
        }

        public void ShowMessage(string content, bool isKey = false)
        {
            _messageText.text = isKey ? _hudMessagesTable.GetTable().GetEntry(content).Value : content;
            ShowMessage();
        }

        private void ShowMessage()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            _container.SetActive(true);
            _messageText.alpha = 1f;
            _panel.localScale = new Vector2(0.5f, 0.5f);

            var color = _panelImage.color;
            color.a = 0f;
            _panelImage.color = color;

            LeanTween
                .scale(_panel, Vector3.one, 1.5f)
                .setEase(LeanTweenType.easeOutElastic);
            LeanTween.alpha(_panel, 1f, 0.3f);

            _fadeCoroutine = StartCoroutine(FadeOutAfterTime());
        }

        private IEnumerator FadeOutAfterTime()
        {
            yield return new WaitForSeconds(4);

            _messageText.alpha = 0f;
            LeanTween.alpha(_panel, 0f, 0.6f);

            yield return new WaitForSeconds(1f);

            _messageText.alpha = 1f;
            _container.SetActive(false);
        }
    }
}