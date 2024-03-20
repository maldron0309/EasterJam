using System.Collections;
using System.Linq;
using mono.objects;
using UnityEngine;
using UnityEngine.Assertions;

namespace mono.player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float _moveSpeed = 6f;

        [SerializeField] private float _movementSmoothing = .05f;
        [SerializeField] private bool _canMove = true;

        [SerializeField] private float _sprintMultiplier = 1.5f;

        [Header("Jump Settings")] [SerializeField]
        private float _jumpForce = 7f;

        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _groundCheckRadius = 0.2f;

        [Header("Glide Settings")] [SerializeField]
        private float _glideGravityScale = 0.5f;

        [SerializeField] private float _normalGravityScale = 1f;
        [SerializeField] private float _glideFallSpeed = -1f;

        [Header("Interaction Settings")] [SerializeField]
        private float _interactionRadius = 1f;

        [SerializeField] private LayerMask _interactableLayerMask;
        private bool _attemptsGliding;
        private bool _attemptsSprinting;
        private bool _canGlide;
        private bool _isGrounded;
        private bool _jumpIsCoolingDown;
        private Vector2 _moveInput;
        private Rigidbody2D _rigidbody2D;
        private Vector3 _velocity;
        public bool CanSpawnCheckpoint => _canMove && _isGrounded;

        public static PlayerMovement Instance { get; private set; }

        private void Awake()
        {
            InitializeComponents();
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            Move();
            HandleGlide();
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheckPoint != null) Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _interactionRadius);
        }

        public void AllowMovement(bool allow)
        {
            _canMove = allow;
        }

        public void TryInteractWithObject()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            var hits = Physics2D.OverlapCircleAll(transform.position, _interactionRadius,
                _interactableLayerMask);

            foreach (var hit in hits.Where(it => it.gameObject.CompareTag("Interactable")))
                if (hit.gameObject.TryGetComponent<InteractableObject>(out var interactableObject))
                    interactableObject.Interact();
        }

        private void InitializeComponents()
        {
            Assert.IsNull(Instance, "Player singleton already exists!");
            Instance = this;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void AttemptMovement(Vector2 movement)
        {
            _moveInput = movement;
        }

        public void AttemptSprint(bool attempt)
        {
            _attemptsSprinting = attempt;
        }

        public void AttemptGlide(bool attempt)
        {
            _attemptsGliding = attempt;
        }

        private void Move()
        {
            if (!_canMove) return;

            var rigidBodyVelocity = _rigidbody2D.velocity;
            var currentSpeed = _attemptsSprinting && _isGrounded ? _moveSpeed * _sprintMultiplier : _moveSpeed;
            var movementVelocity = new Vector2(_moveInput.x * currentSpeed * Time.fixedDeltaTime, rigidBodyVelocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp
            (
                rigidBodyVelocity,
                movementVelocity,
                ref _velocity,
                _movementSmoothing
            );
        }

        public void TryJump()
        {
            if (!_isGrounded || !_canMove || _jumpIsCoolingDown) return;

            _isGrounded = false;
            _rigidbody2D.AddForce(new(0f, _jumpForce));

            StartCoroutine(StartJumpCooldown());
            StartCoroutine(StartAllowGlide());
        }

        private IEnumerator StartJumpCooldown()
        {
            _jumpIsCoolingDown = true;
            yield return new WaitForSeconds(0.05f);
            _jumpIsCoolingDown = false;
        }

        private IEnumerator StartAllowGlide()
        {
            yield return new WaitForSeconds(0.3f);
            _canGlide = true;
        }

        private void HandleGlide()
        {
            if (_attemptsGliding && _canGlide && !_isGrounded)
            {
                var rigidBodyVelocity = _rigidbody2D.velocity;

                _rigidbody2D.gravityScale = _glideGravityScale;
                var yVelocity = Mathf.Max(rigidBodyVelocity.y, _glideFallSpeed);
                var movementVelocity = new Vector2(rigidBodyVelocity.x, yVelocity);

                _rigidbody2D.velocity = Vector3.SmoothDamp
                (
                    rigidBodyVelocity,
                    movementVelocity,
                    ref _velocity,
                    _movementSmoothing
                );
            }
            else
            {
                _rigidbody2D.gravityScale = _normalGravityScale;
            }
        }

        private void CheckGrounded()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
            if (_isGrounded) _canGlide = false;
        }
    }
}