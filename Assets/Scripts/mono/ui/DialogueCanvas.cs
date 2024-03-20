using System.Collections;
using System.Text.RegularExpressions;
using controller;
using manager;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace mono.ui
{
    public class DialogueCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private GameObject _skipButtonViewContainer;
        [SerializeField] private Image _skipButtonProgressImage;
        [SerializeField] private Color _highlightedPassageColour;
        [SerializeField] private Animator _waitForInputIndicatorAnimator;

        private int _currentLineIndex;
        private LocalizedString[] _currentLines;
        private bool _informCutscenePlayerOnFinish;
        private bool _isRevealingCharacters;

        private Coroutine _revealCoroutine;
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
            _skipButtonProgressImage.fillAmount += 0.03f;
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
                var text = _currentLines[_currentLineIndex].GetLocalizedString();
                var amountOfHighlightedPassages = 0;

                const string highlightedPassagePattern = @"\[(.*?)\]";

                var replacedText = Regex.Replace(text, highlightedPassagePattern, match =>
                {
                    var matchValue = match.Groups[1].Value;
                    amountOfHighlightedPassages++;
                    return $"<color=#{ColorUtility.ToHtmlStringRGB(_highlightedPassageColour)}>{matchValue}</color>";
                });

                _waitForInputIndicatorAnimator.SetTrigger("Toggle");

                _dialogueText.text = replacedText;
                _revealCoroutine = StartCoroutine(RevealCharacters(
                    text.Length - amountOfHighlightedPassages * 2));
            }
            else
            {
                OnDialogueFinish();
            }
        }

        private IEnumerator RevealCharacters(int textPartLength)
        {
            _isRevealingCharacters = true;

            var characterCounter = 0;

            while (true)
            {
                var visibleCounter = characterCounter % (textPartLength + 1);
                _dialogueText.maxVisibleCharacters = visibleCounter;

                if (visibleCounter >= textPartLength)
                {
                    _isRevealingCharacters = false;
                    _waitingForProceed = true;

                    _waitForInputIndicatorAnimator.SetTrigger("Toggle");

                    yield break;
                }

                characterCounter++;

                yield return new WaitForSeconds(0.03f);
            }
        }

        private void OnDialogueFinish()
        {
            StopCoroutine(_revealCoroutine);

            _currentLineIndex = -1;
            _currentLines = null;
            _waitingForProceed = false;

            if (_informCutscenePlayerOnFinish) CutsceneManager.Instance.ProceedWithNextSegment();

            _informCutscenePlayerOnFinish = false;
            _container.gameObject.SetActive(false);
        }

        private bool HasNextLine() => _currentLineIndex < _currentLines.Length - 1;
    }
}