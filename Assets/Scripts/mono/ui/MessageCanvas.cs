using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization;

namespace mono.ui
{
    public class MessageCanvas : MonoBehaviour
    {
        public const string CHECKPOINT_SPAWNED_KEY_NAME = "checkpoint-spawned";
        public const string NOT_ENOUGH_CHECKPOINT_MANA_KEY_NAME = "not-enough-checkpoint-mana";

        [SerializeField] private LocalizedStringTable _hudMessagesTable;
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _messageText;

        public static MessageCanvas Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;
        }

        public void ShowMessage(string keyName)
        {
            _panel.transform.localScale = new Vector2(0.5f, 0.5f);
            LeanTween
                .scale(_panel, Vector3.one, 1.5f)
                .setEase(LeanTweenType.easeOutElastic);

            _messageText.text = _hudMessagesTable.GetTable().GetEntry(keyName).Value;
        }
    }
}