using entity;
using manager;
using UnityEngine;

namespace mono.objects
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] private CutsceneSO _cutsceneToTrigger;

        public void TriggerCutscene()
        {
            if (_cutsceneToTrigger == null) return;
            CutsceneManager.Instance.Play(_cutsceneToTrigger);
        }
    }
}