using manager;
using mono.ui;
using UnityEngine;

namespace entity
{
    [CreateAssetMenu(menuName = "Data/Cutscene/Letterbox Segment", fileName = "New Letterbox Segment")]
    public class LetterboxCutsceneSegment : CutsceneSegment
    {
        [SerializeField] private bool _show;

        public override void Execute()
        {
            CutsceneLetterboxCanvas.Instance.Toggle(_show);
            CutsceneManager.Instance.ProceedWithNextSegment();
        }
    }
}