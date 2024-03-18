using UnityEngine;

namespace manager
{
    public class EssentialsManager : MonoBehaviour
    {
        public static bool HasLoaded { get; private set; }

        private void Awake()
        {
            HasLoaded = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}