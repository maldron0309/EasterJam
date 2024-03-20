using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace mono.objects
{
    public class MovedPlatform : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Transform[] _movementPoints;
        [SerializeField] private int _startingMovementPoint;

        [SerializeField] private UnityEvent _onMovementStarted;
        [SerializeField] private UnityEvent _onMovementStopped;


        private int _currentPointIndex;

        private bool _hasQuit;
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

            transform.position = Vector2.MoveTowards
            (
                transform.position,
                _movementPoints[_currentPointIndex].position,
                _movementSpeed * Time.deltaTime
            );
        }

        private void OnApplicationQuit()
        {
            _hasQuit = true;
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player")) other.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            // Fixes errors showing up when quitting whilst the player is colliding with the platform
            if (_hasQuit) return;

            if (other.gameObject.CompareTag("Player")) StartCoroutine(ResetParentAfterFrame(other.gameObject));
        }

        private static IEnumerator ResetParentAfterFrame(GameObject other)
        {
            yield return null;
            other.transform.SetParent(null);
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

        public void ToggleMovement()
        {
            _isMoving = !_isMoving;

            if (_isMoving) _onMovementStarted.Invoke();
            else _onMovementStopped.Invoke();
        }
    }
}