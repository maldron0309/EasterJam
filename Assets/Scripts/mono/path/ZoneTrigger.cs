using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace mono.path
{
    public class ZoneTrigger : MonoBehaviour
    {
        [Tag] [SerializeField] private string _allowedColliderTagName;
        [SerializeField] private UnityEvent _onZoneTriggered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_allowedColliderTagName)) return;
            _onZoneTriggered.Invoke();
        }
    }
}