using UnityEngine;

namespace entity
{
    [CreateAssetMenu(menuName = "Data/Cutscene/Cutscene", fileName = "New Cutscene")]
    public class CutsceneSO : ScriptableObject
    {
        [SerializeField] private CutsceneSegment[] _segments;

        public int AmountOfSegments => _segments.Length;

        public CutsceneSegment SegmentAt(int currentSegmentIndex) => _segments[currentSegmentIndex];
    }
}