using service;
using UnityEngine;

namespace manager
{
    public class GameLoadManager : MonoBehaviour
    {
        private void Awake()
        {
            CreateSingletonServices();
        }

        private static void CreateSingletonServices()
        {
            new PlayerHealthService();
            new CheckpointService();
            // ...
        }
    }
}