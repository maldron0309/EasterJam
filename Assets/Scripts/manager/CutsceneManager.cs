using entity;
using UnityEngine;
using UnityEngine.Assertions;

namespace manager
{
    public class CutsceneManager : MonoBehaviour
    {
        private int _currentSegmentIndex;
        private CutsceneSO _playedCutscene;
        public static CutsceneManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "Cutscene manager singleton already exists!");
            Instance = this;
        }

        public void Play(CutsceneSO cutsceneToPlay)
        {
            _currentSegmentIndex = -1;
            _playedCutscene = cutsceneToPlay;

            ProceedWithNextSegment();
        }

        public void ProceedWithNextSegment()
        {
            if (HasNextSegment())
            {
                _currentSegmentIndex++;
                _playedCutscene.SegmentAt(_currentSegmentIndex).Execute();
            }
            else
            {
                OnCutsceneFinished();
            }
        }

        private void OnCutsceneFinished()
        {
            _currentSegmentIndex = -1;
            _playedCutscene = null;
        }

        private bool HasNextSegment() =>
            _playedCutscene != null && _currentSegmentIndex < _playedCutscene.AmountOfSegments - 1;
    }
}