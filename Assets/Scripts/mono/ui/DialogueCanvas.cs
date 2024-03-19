using System.Collections;
using controller;
using manager;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace mono.ui
{
    public class DialogueCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private GameObject _skipButtonViewContainer;
        [SerializeField] private Image _skipButtonProgressImage;
        [SerializeField] private LocalizeStringEvent _skipTextEvent;

        private Coroutine _cooldownCoroutine;
        private int _currentLineIndex;

        private LocalizedString[] _currentLines;
        private bool _informCutscenePlayerOnFinish;
        private bool _waitingForProceed;

        public static DialogueCanvas Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "Dialogue Canvas singleton already exists!");
            Instance = this;
        }

        private void FixedUpdate()
        {
            if (!_skipButtonViewContainer.activeInHierarchy) return;
            _skipButtonProgressImage.fillAmount += 0.028f;
        }

        private void OnEnable()
        {
            InputController.Instance.InputMaster.Dialogue.Proceed.performed += Proceed;
            InputController.Instance.InputMaster.Dialogue.Skip.performed += Skip;
            InputController.Instance.InputMaster.Dialogue.Skip.started += ShowSkipButtonView;
            InputController.Instance.InputMaster.Dialogue.Skip.canceled += HideSkipButtonView;
        }

        private void OnDisable()
        {
            InputController.Instance.InputMaster.Dialogue.Proceed.performed -= Proceed;
            InputController.Instance.InputMaster.Dialogue.Skip.performed -= Skip;
            InputController.Instance.InputMaster.Dialogue.Skip.started -= ShowSkipButtonView;
            InputController.Instance.InputMaster.Dialogue.Skip.canceled -= HideSkipButtonView;
        }

        private void HideSkipButtonView(InputAction.CallbackContext _)
        {
            _skipButtonProgressImage.fillAmount = 0f;
            _skipButtonViewContainer.gameObject.SetActive(false);
        }

        private void ShowSkipButtonView(InputAction.CallbackContext _)
        {
            _skipButtonViewContainer.gameObject.SetActive(true);
        }

        private void Skip(InputAction.CallbackContext obj)
        {
            OnDialogueFinish();
        }

        private void Proceed(InputAction.CallbackContext _)
        {
            if (!_waitingForProceed) return;
            _waitingForProceed = false;
            ShowNextLine();
        }

        public void StartDialogue(LocalizedString[] dialogueLines, bool informCutscenePlayer = false)
        {
            _container.gameObject.SetActive(true);
            _currentLineIndex = -1;
            _currentLines = dialogueLines;
            _waitingForProceed = false;
            _informCutscenePlayerOnFinish = informCutscenePlayer;

            ShowNextLine();
        }

        private void ShowNextLine()
        {
            if (HasNextLine())
            {
                _currentLineIndex++;
                _dialogueText.text = _currentLines[_currentLineIndex].GetLocalizedString();
                _cooldownCoroutine = StartCoroutine(ShowNextAfterCooldown());
            }
            else
            {
                OnDialogueFinish();
            }
        }

        private void OnDialogueFinish()
        {
            StopCoroutine(_cooldownCoroutine);

            _currentLineIndex = -1;
            _currentLines = null;
            _waitingForProceed = false;

            if (_informCutscenePlayerOnFinish) CutsceneManager.Instance.ProceedWithNextSegment();

            _informCutscenePlayerOnFinish = false;
            _container.gameObject.SetActive(false);
        }

        private IEnumerator ShowNextAfterCooldown()
        {
            yield return new WaitForSeconds(3f);
            _waitingForProceed = true;
        }

        private bool HasNextLine() => _currentLineIndex < _currentLines.Length - 1;
    }
}