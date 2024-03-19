using mono.ui;
using UnityEngine;
using UnityEngine.Localization;

namespace mono.objects
{
    public class InfoZone : MonoBehaviour
    {
        [SerializeField] private bool _allowOnceOnly;
        [SerializeField] private LocalizedString _messageString;

        private bool _hasBeenTriggeredOnce;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") && _hasBeenTriggeredOnce && _allowOnceOnly) return;
            _hasBeenTriggeredOnce = true;

            MessageCanvas.Instance.ShowMessage(_messageString);
        }
    }
}