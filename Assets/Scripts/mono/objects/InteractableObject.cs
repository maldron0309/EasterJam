using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace mono.objects
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onInteraction;
        [SerializeField] private float _cooldownTime = 2f;

        private bool _inCooldown;

        public void Interact()
        {
            if (_inCooldown) return;
            _onInteraction.Invoke();
            StartCoroutine(StartCooldown());
        }

        private IEnumerator StartCooldown()
        {
            _inCooldown = true;
            yield return new WaitForSeconds(_cooldownTime);
            _inCooldown = false;
        }
    }
}