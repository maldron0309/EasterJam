using UnityEngine;
using UnityEngine.Events;

namespace mono.ui
{
    public class EventEmitter : MonoBehaviour
    {
        [SerializeField] private UnityEvent _event;

        public void Emit()
        {
            _event.Invoke();
        }
    }
}