using System.Collections;
using manager;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization;

namespace mono.ui
{
    public class DialogueCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _dialogueText;
        private int _currentLineIndex;

        private LocalizedString[] _currentLines;
        private bool _informCutscenePlayerOnFinish;

        public static DialogueCanvas Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "Dialogue Canvas singleton already exists!");
            Instance = this;
        }

        public void StartDialogue(LocalizedString[] dialogueLines, bool informCutscenePlayer = false)
        {
            _container.gameObject.SetActive(true);
            _currentLineIndex = -1;
            _currentLines = dialogueLines;
            _informCutscenePlayerOnFinish = informCutscenePlayer;

            ShowNextLine();
        }

        private void ShowNextLine()
        {
            if (HasNextLine())
            {
                _currentLineIndex++;
                _dialogueText.text = _currentLines[_currentLineIndex].GetLocalizedString();
                StartCoroutine(ShowNextAfterCooldown());
            }
            else
            {
                OnDialogueFinish();
            }
        }

        private void OnDialogueFinish()
        {
            _currentLineIndex = -1;
            _currentLines = null;

            if (_informCutscenePlayerOnFinish) CutsceneManager.Instance.ProceedWithNextSegment();

            _informCutscenePlayerOnFinish = false;
            _container.gameObject.SetActive(false);
        }

        private IEnumerator ShowNextAfterCooldown()
        {
            yield return new WaitForSeconds(3f);
            ShowNextLine();
        }

        private bool HasNextLine() => _currentLineIndex < _currentLines.Length - 1;
    }
}