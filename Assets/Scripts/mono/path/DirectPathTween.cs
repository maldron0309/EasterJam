using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace mono.path
{
    public class DirectPathTween : MonoBehaviour
    {
        [SerializeField] private Transform _destinationTransform;
        [SerializeField] private float _time;
        [SerializeField] private UnityEvent _onDestinationReached;
        [SerializeField] private UnityEvent _onStartPath;

        [FormerlySerializedAs("_type")] [SerializeField]
        private LeanTweenType _easeType;

        public void SetDestination(Transform destinationTransform) => _destinationTransform = destinationTransform;

        public void Move()
        {
            _onStartPath.Invoke();

            LeanTween
                .move(gameObject, _destinationTransform, _time)
                .setEase(_easeType)
                .setOnComplete(_onDestinationReached.Invoke);
        }
    }
}