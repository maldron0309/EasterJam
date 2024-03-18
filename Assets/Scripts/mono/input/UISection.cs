using UnityEngine;

namespace mono.input
{
    public class UISection : MonoBehaviour
    {
        [SerializeField] private GameObject _selectedStartObjectOnControlledInput;

        public GameObject SelectedStartObjectOnControlledInput => _selectedStartObjectOnControlledInput;

        public static UISection Current { private set; get; }

        public void Open()
        {
            Current = this;
        }
    }
}