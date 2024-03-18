using UnityEngine;
using UnityEngine.Events;

namespace mono.ui
{
    public class ApplicationQuitter : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onAbortedInstead;

        public void Abort()
        {
            _onAbortedInstead.Invoke();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}