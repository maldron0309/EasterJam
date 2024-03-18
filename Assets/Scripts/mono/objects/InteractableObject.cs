using UnityEngine;
using UnityEngine.Events;

namespace mono.objects
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onInteraction;

        public void Interact()
        {
            Debug.Log("Interacting");
            _onInteraction.Invoke();
        }
    }
}