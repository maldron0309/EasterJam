using manager;
using mono.input;
using UnityEngine;
using UnityEngine.UI;

namespace mono.ui
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Image _transitionImage;
        [SerializeField] private UISection _mainMenuSection;

        private void Awake()
        {
            _mainMenuSection.Open();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _transitionImage.color = Color.black;
            LeanTween
                .alpha(_transitionImage.rectTransform, 0f, 4f)
                .setEase(LeanTweenType.linear)
                .setDelay(0.1f);
        }

        public void LoadGame()
        {
            UIInputLayoutManager.SetGamepadColour(Color.green);
        }
    }
}