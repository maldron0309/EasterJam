using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
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
    private bool _canGlide;
    private bool _isGrounded;
    private bool _isSprinting;
    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _velocity;

    private InputMaster controls;

    private void Awake()
    {
        InitializeComponents();
        SetupInputActions();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Move();
        HandleGlide();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheckPoint != null) Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }

    private void InteractWithObject()
    {
        // ReSharper disable once Unity.PreferNonAllocApi
        var hits = Physics2D.OverlapCircleAll(transform.position, _interactionRadius, _interactableLayerMask);
        foreach (var hit in hits.Where(hit => CompareTag("Interactable")))
            Debug.Log("Interacted with " + hit.name);


        // TODO Implement interaction logic here
    }

    private void InitializeComponents()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void SetupInputActions()
    {
        controls = new();
        controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
        controls.Player.Sprint.performed += ctx => _isSprinting = true;
        controls.Player.Sprint.canceled += ctx => _isSprinting = false;
        controls.Player.Jump.performed += ctx => AttemptJump();
        controls.Player.Glide.performed += ctx => _attemptsGliding = true;
        controls.Player.Glide.canceled += ctx => _attemptsGliding = false;
        controls.Player.Interact.performed += ctx => InteractWithObject();
    }

    private void Move()
    {
        if (!_canMove) return;

        var rigidBodyVelocity = _rigidbody2D.velocity;
        var currentSpeed = _isSprinting && _isGrounded ? _moveSpeed * _sprintMultiplier : _moveSpeed;
        var movementVelocity = new Vector2(_moveInput.x * currentSpeed * Time.fixedDeltaTime, rigidBodyVelocity.y);
        _rigidbody2D.velocity = Vector3.SmoothDamp
        (
            rigidBodyVelocity,
            movementVelocity,
            ref _velocity,
            _movementSmoothing
        );
    }

    private void AttemptJump()
    {
        if (!_isGrounded) return;

        _isGrounded = false;
        _rigidbody2D.AddForce(new(0f, _jumpForce));
        StartCoroutine(StartAllowGlide());
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