using UnityEngine;
using UnityEngine.Assertions;

namespace mono.ui
{
    public class CutsceneLetterboxCanvas : MonoBehaviour
    {
        [SerializeField] private Animator _containerAnimator;
        public static CutsceneLetterboxCanvas Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "Cutscene Letterbox canvas singleton already exists");
            Instance = this;
        }

        public void Toggle(bool show)
        {
            if (show) _containerAnimator.gameObject.SetActive(true);
            else _containerAnimator.SetTrigger("Out");
        }
    }
}