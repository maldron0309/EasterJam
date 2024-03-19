using System.Collections;
using controller;
using manager;
using mono.input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace mono.ui
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Image _transitionImage;
        [SerializeField] private UISection _keyPressSection;
        [SerializeField] private UISection _mainMenuSection;

        private bool _inMinimumWaitTime;

        private void Awake()
        {
            StartCoroutine(StartMinimumWaitingTime());
        }

        // Start is called before the first frame update
        private void Start()
        {
            _transitionImage.color = Color.black;
            LeanTween
                .alpha(_transitionImage.rectTransform, 0f, 4f)
                .setEase(LeanTweenType.linear)
                .setDelay(0.1f);
        }

        private void OnEnable()
        {
            InputController.Instance.InputMaster.InputLayout.SwitchToControlled.performed += ProceedToMenu;
            InputController.Instance.InputMaster.InputLayout.SwitchToMouse.performed += ProceedToMenu;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.InputLayout.SwitchToControlled.performed -= ProceedToMenu;
            InputController.Instance.InputMaster.InputLayout.SwitchToMouse.performed -= ProceedToMenu;
        }

        private IEnumerator StartMinimumWaitingTime()
        {
            _inMinimumWaitTime = true;
            yield return new WaitForSeconds(2f);
            _inMinimumWaitTime = false;
        }

        private void ProceedToMenu(InputAction.CallbackContext _)
        {
            if (_inMinimumWaitTime) return;

            _keyPressSection.gameObject.SetActive(false);
            _mainMenuSection.gameObject.SetActive(true);

            InputController.Instance.InputMaster.InputLayout.SwitchToControlled.performed -= ProceedToMenu;
            InputController.Instance.InputMaster.InputLayout.SwitchToMouse.performed -= ProceedToMenu;

            _mainMenuSection.Open();
        }

        public void LoadGame()
        {
            UIInputLayoutManager.SetGamepadColour(Color.green);
            StartCoroutine(DisableSelf());
        }

        private IEnumerator DisableSelf()
        {
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }
    }
}