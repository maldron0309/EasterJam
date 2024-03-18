using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace mono.entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Cloud : MonoBehaviour
    {
        private const float MIN_MOVEMENT_SPEED = 65f;
        private const float MAX_MOVEMENT_SPEED = 85f;
        private const float MIN_DELAY_TIME = 0f;
        private const float MAX_DELAY_TIME = 2f;

        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private Transform _startTransform;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Transform _maxHeightPoint;
        [SerializeField] private Transform _minHeightPoint;

        private SpriteRenderer _spriteRenderer;


        // Start is called before the first frame update
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            SimulateMovement();
        }

        private void SimulateMovement()
        {
            var movementSpeed = Random.Range(MIN_MOVEMENT_SPEED, MAX_MOVEMENT_SPEED);

            LeanTween
                .moveX(gameObject, _targetTransform.position.x, movementSpeed)
                .setOnComplete(() => StartCoroutine(OnTargetReached()));
        }

        private IEnumerator OnTargetReached()
        {
            transform.position = new Vector2(_startTransform.position.x,
                Random.Range(_minHeightPoint.position.y, _maxHeightPoint.position.y));

            var size = Random.Range(_minSize, _maxSize);
            transform.localScale = new Vector2(size, size);

            _spriteRenderer.flipX = Random.Range(0, 2) is 1;

            yield return new WaitForSeconds(Random.Range(MIN_DELAY_TIME, MAX_DELAY_TIME));

            SimulateMovement();
        }
    }
}