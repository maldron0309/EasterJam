using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace mono.ui
{
    public class MessageCanvas : MonoBehaviour
    {
        public const string CHECKPOINT_SPAWNED_KEY_NAME = "checkpoint-spawned";
        public const string NOT_ENOUGH_CHECKPOINT_MANA_KEY_NAME = "not-enough-checkpoint-mana";

        [SerializeField] private LocalizedStringTable _hudMessagesTable;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Image _panelImage;


        private Coroutine _fadeCoroutine;

        public static MessageCanvas Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;
        }

        public void ShowMessage(LocalizedString message)
        {
            _messageText.text = message.GetLocalizedString();
            ShowMessage();
        }

        public void ShowMessage(string keyName)
        {
            _messageText.text = _hudMessagesTable.GetTable().GetEntry(keyName).Value;
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