using UnityEngine;

namespace mono.objects
{
    public class MovedPlatform : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Transform[] _movementPoints;
        [SerializeField] private int _startingMovementPoint;

        private int _currentPointIndex;
        private bool _isMoving;

        private void Awake()
        {
            MoveToStartingPosition();
        }

        private void Update()
        {
            if (!_isMoving) return;

            if (Vector2.Distance(transform.position, _movementPoints[_currentPointIndex].position) < 0.02f)
                OnDestinationReached();

            transform.position = Vector2.MoveTowards(transform.position, _movementPoints[_currentPointIndex].position,
                _movementSpeed * Time.deltaTime);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player")) other.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player")) other.gameObject.transform.SetParent(null);
        }

        private void MoveToStartingPosition()
        {
            transform.position = _movementPoints[_startingMovementPoint].position;
        }

        private void OnDestinationReached()
        {
            _currentPointIndex++;
            if (_currentPointIndex == _movementPoints.Length) _currentPointIndex = 0;
        }

        public void StartMoving()
        {
            _isMoving = true;
        }
    }
}