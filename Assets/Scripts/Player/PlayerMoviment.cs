using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMoviment : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpForce = 15f;

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsulecollider2D;

    private float moveInput;
    private bool isGrounded;
    private bool wasInAir;
    private bool isLanding;
    private bool isJumping;
    private bool isTouchingWall;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private BoxCollider2D playerWallColliderCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsulecollider2D = GetComponent<CapsuleCollider2D>();
    }


    void Update()
    {
        if (isLanding) return; // Prevent movement during landing animation

        moveInput = Input.GetAxisRaw("Horizontal");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (!isGrounded)
        {
            isRunning = false;
        }

        float currentSpeed = isRunning ? runSpeed : speed;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        //Flip the player sprite based on movement direction
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
        }

        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Check if the player is touching a wall
        isTouchingWall = playerWallColliderCheck.IsTouchingLayers(groundLayer);

        
        if (isTouchingWall)
        {
            capsulecollider2D.sharedMaterial.friction = 0f; // Set friction to 0 when touching a wall
            Debug.Log("Touching wall, friction set to 0");
        }
        else
        {
            capsulecollider2D.sharedMaterial.friction = 1.0f; // Reset friction when not touching a wall
            Debug.Log("Not touching wall, friction reset to 1.0");
        }

        //Detect Landing
        if (!isGrounded)
        {
            wasInAir = true;
        }
        if (isGrounded && wasInAir)
        {
            isLanding = true;
            wasInAir = false;
            animator.SetTrigger("Land");
        }

        updateAnimations(moveInput, isRunning);
    }

    void updateAnimations(float moveInput, bool isRunning)
    {
        animator.SetBool("isIdle", moveInput == 0);
        animator.SetBool("isWalking", moveInput != 0 && !isRunning);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);

        if (isJumping)
        {
            animator.SetTrigger("isJumping");
            isJumping = false;
        }

    }

    public void endLanding()
    {
        isLanding = false;
    }
}
