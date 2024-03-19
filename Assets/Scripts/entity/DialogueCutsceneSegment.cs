using mono.ui;
using UnityEngine;
using UnityEngine.Localization;

namespace entity
{
    [CreateAssetMenu(fileName = "Dialogue Segment", menuName = "Data/Cutscene/Dialogue Segment", order = 0)]
    public class DialogueCutsceneSegment : CutsceneSegment
    {
        [SerializeField] private LocalizedString[] _dialogueLines;

        public override void Execute()
        {
            DialogueCanvas.Instance.StartDialogue(_dialogueLines, true);
        }
    }
}