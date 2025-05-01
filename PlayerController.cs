using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AdvancedPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private string runAnimParam = "isRunning";
    [SerializeField] private string jumpAnimParam = "isJumping";

    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isJumping;
    
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleInput();
        HandleJump();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        // Coyote Time
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        // Jump logic
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            isJumping = true;
        }

        // Reset jump when landing
        if (IsGrounded() && rb.linearVelocity.y <= 0)
        {
            isJumping = false;
        }
    }

    private void HandleMovement()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Flip character based on movement direction
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // Running animation
        animator.SetBool(runAnimParam, horizontalInput != 0);

        // Jumping animation (true when in air, false when grounded)
        animator.SetBool(jumpAnimParam, !IsGrounded());
    }

    private bool IsGrounded()
    {
        bool raycastHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        bool overlapHit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        return raycastHit || overlapHit;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}