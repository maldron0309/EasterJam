using mono.spawnables;
using UnityEngine;
using UnityEngine.Events;

namespace mono.spawner
{
    public class CheckpointSpawner : MonoBehaviour
    {
        [SerializeField] private Checkpoint _checkpointPrefab;
        [SerializeField] private UnityEvent _onCheckpointSpawned;


        private Checkpoint _checkpoint;

        public static CheckpointSpawner Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            // When a new level is loaded, the old checkpoint is deleted
            // We can change this later to serialise the data if we want
            _checkpoint = null;
        }

        /// <summary>
        ///     Moves the checkpoint to the player or creates a new checkpoint if there was no checkpoint before
        /// </summary>
        public void SpawnCheckpoint()
        {
            // Pool the checkpoint object
            if (_checkpoint == null) _checkpoint = Instantiate(_checkpointPrefab);

            // Move to the player position
            _checkpoint.transform.position = transform.position;

            _onCheckpointSpawned.Invoke();
        }
    }
}