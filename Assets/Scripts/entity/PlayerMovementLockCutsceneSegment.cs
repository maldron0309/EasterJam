using manager;
using mono.player;
using UnityEngine;

namespace entity
{
    [CreateAssetMenu(fileName = "Player Movement Lock Segment", menuName = "Data/Cutscene/Player Movement Lock Segment",
        order = 0)]
    public class PlayerMovementLockCutsceneSegment : CutsceneSegment
    {
        [SerializeField] private bool _enable;

        public override void Execute()
        {
            PlayerMovement.Instance.AllowMovement(_enable);
            CutsceneManager.Instance.ProceedWithNextSegment();
        }
    }
}