using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float sprintMultiplier = 1.5f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;

    [Header("Glide Settings")]
    public float glideGravityScale = 0.5f;
    public float normalGravityScale = 1f;
    public float glideFallSpeed = -1f;

    [Header("Interaction Settings")]
    public float interactionRadius = 1f;
    public LayerMask interactableLayerMask;

    private InputMaster controls;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isSprinting = false;
    private bool isGrounded;
    private bool isGliding = false;

    private void Awake()
    {
        InitializeComponents();
        SetupInputActions();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Move();
        HandleGlide();
    }

    public void InteractWithObject()
    {
        Debug.Log("InteractWithObject()");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayerMask);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Interactable"))
            {
                Debug.Log("Interacted with " + hit.name);
                // TODO Implement interaction logic here
            }
        }
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void SetupInputActions()
    {
        controls = new InputMaster();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Sprint.performed += ctx => isSprinting = true;
        controls.Player.Sprint.canceled += ctx => isSprinting = false;
        controls.Player.Jump.performed += ctx => AttemptJump();
        controls.Player.Glide.performed += ctx => isGliding = true;
        controls.Player.Glide.canceled += ctx => isGliding = false;
        controls.Player.Interact.performed += ctx => InteractWithObject();
    }

    private void Move()
    {
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector2 movement = new Vector2(moveInput.x * currentSpeed, rb.velocity.y);
        rb.velocity = movement;
    }

    private void AttemptJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void HandleGlide()
    {
        if (isGliding && !isGrounded)
        {
            rb.gravityScale = glideGravityScale;
            float yVelocity = Mathf.Max(rb.velocity.y, glideFallSpeed);
            rb.velocity = new Vector2(rb.velocity.x, yVelocity);
        }
        else
        {
            rb.gravityScale = normalGravityScale;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
