using UnityEngine;

namespace entity
{
    public abstract class CutsceneSegment : ScriptableObject
    {
        public abstract void Execute();
    }
}