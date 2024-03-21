using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace mono.input
{
    public class UISection : MonoBehaviour
    {
        [SerializeField] private GameObject _selectedStartObjectOnControlledInput;
        [SerializeField] private bool _enableConfirmation;
        [SerializeField] private bool _enableAbortion;

        [SerializeField] private UnityEvent _onOpened;

        [ShowIf("_enableConfirmation")] [SerializeField]
        private UnityEvent _onConfirmation;

        [ShowIf("_enableAbortion")] [SerializeField]
        private UnityEvent _onAbortion;

        public GameObject SelectedStartObjectOnControlledInput => _selectedStartObjectOnControlledInput;

        public static UISection Current { private set; get; }

        public void TryConfirmation()
        {
            if (!_enableConfirmation) return;
            _onConfirmation.Invoke();
        }

        public void TryAbortion()
        {
            if (!_enableAbortion) return;
            _onAbortion.Invoke();
        }

        public void Open()
        {
            Current = this;

            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_selectedStartObjectOnControlledInput);

            _onOpened.Invoke();
        }
    }
}