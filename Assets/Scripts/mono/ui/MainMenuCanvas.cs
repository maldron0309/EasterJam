using UnityEngine;
using UnityEngine.UI;

namespace mono.ui
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Image _transitionImage;

        // Start is called before the first frame update
        private void Start()
        {
            _transitionImage.color = Color.black;
            LeanTween
                .alpha(_transitionImage.rectTransform, 0f, 2f);
        }
    }
}