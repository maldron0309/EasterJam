using UnityEngine;

namespace mono.spawnables
{
    public class Checkpoint : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlaySpawnAnimation() => _animator.Play("Checkpoint Spawn");
    }
}