using UnityEngine;

namespace manager
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject _essentials;

        private void Awake()
        {
            // Load the essentials if they don't exist
            if (EssentialsManager.HasLoaded) return;
            Instantiate(_essentials);
        }
    }
}