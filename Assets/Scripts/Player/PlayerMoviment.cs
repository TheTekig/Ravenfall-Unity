using System;
using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMoviment : MonoBehaviour
{
    #region SPEED AND JUMPING VARIABLES
    [SerializeField] float speed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpForce = 8f;
    #endregion

    #region COMPONENTS
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsulecollider2D;
    #endregion

    #region CHECKING VARIABLES
    private float moveInput;
    private bool isGrounded;
    private bool wasInAir;
    private bool isLanding;
    private bool isJumping;
    private bool isTouchingWall;
    #endregion

    #region WALL JUMP / JUMP VARIABLES
    [SerializeField] float jumpBufferTime = 0.15f;
    [SerializeField] float coyoteTime = 0.15f;
    private float jumpBufferTimer;
    private float coyoteTimer;
    private bool canDoubleJump;

    [SerializeField] float wallJumpForceX = 8f;
    [SerializeField] float wallJumpForceY = 8f;
    [SerializeField] float wallJumpLockTime = 0.2f;
    private bool isWallJumping;
    private float wallJumpTimer;
    #endregion

    private float highestY;

    #region FRICTION MATERIALS VARIABLES
    [SerializeField] PhysicsMaterial2D wallFrictionlessMaterial;
    [SerializeField] PhysicsMaterial2D defaultFrictionMaterial;
    #endregion


    [SerializeField] private Transform groundCheck;
    [SerializeField] private BoxCollider2D playerWallCollider;
    [SerializeField] private LayerMask groundLayer;


    #region STAMINA SECTION
    [SerializeField] Slider staminaSlider;
    [SerializeField] GameObject staminaUI;
    [SerializeField] float hideDelay = 1.5f;
    private float hideTimer;

    [SerializeField] float maxStamina = 5f;
    [SerializeField] float stamina;

    [SerializeField] float staminaDrain = 1.5f;
    [SerializeField] float staminaRegen = 1f;
    [SerializeField] float staminaRegenDelay = 1f;

    private float ragenTimer;

    #endregion



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsulecollider2D = GetComponent<CapsuleCollider2D>();

        stamina = maxStamina;
    }


    void Update()
    {
        staminaSlider.value = stamina / maxStamina;

        #region Preventing movement during some actions
        
        if (isLanding) return; // Prevent movement during landing animation

        if(isWallJumping) // Handle wall jump lock timer
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
            }
        }

        if (isWallJumping) return; // Prevent movement during wall jump lock time
        #endregion

        #region Checkig inputs and states
        //Check horizontal input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Check if the player is touching a wall
        isTouchingWall = playerWallCollider.IsTouchingLayers(groundLayer);
        #endregion

        #region Move  - Stamina and Running System
        // Check if the player is running 
        bool canRun = stamina > 0.1f; // Allow running if stamina is above a small threshold
        bool WantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isRunning = canRun && WantsToRun && moveInput != 0 && isGrounded;

        if (isRunning || stamina < maxStamina)
        {
           staminaUI.SetActive(true);
            hideTimer = 0f; // Reset hide timer when running or when stamina is not full
        }
        else
        {
            hideTimer += Time.deltaTime;

            if (hideTimer >= hideDelay)
            {
                staminaUI.SetActive(false);
            }
        }


        if (isRunning && stamina > 0)
        {

            stamina -= staminaDrain * Time.deltaTime;
            ragenTimer = 0f; // Reset regen timer when running
        }
        else
        {
            ragenTimer += Time.deltaTime;
            if (ragenTimer >= staminaRegenDelay)
            {
                stamina += staminaRegen * Time.deltaTime;
            }
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        float currentSpeed = isRunning ? runSpeed : speed;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        //Flip the player sprite based on movement direction
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        #endregion

        #region Jump and Wall Jump System - Landing Detection

        //WallSlide 
        capsulecollider2D.sharedMaterial = isTouchingWall ? wallFrictionlessMaterial : defaultFrictionMaterial;
        bool isWallSliding = isTouchingWall && !isGrounded && moveInput != 0;
        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2f);
        }

        // Jump System
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBufferTime; // Reset jump buffer timer when jump is pressed
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime; // Decrease jump buffer timer over time
        }

      
        if (isGrounded)
        {
            coyoteTimer = coyoteTime; // Reset coyote timer when grounded
            canDoubleJump = true; // Reset double jump when grounded
        }
        else
        {
            coyoteTimer -= Time.deltaTime; // Decrease coyote timer when in the air
        }


        if (jumpBufferTimer > 0 && isWallSliding) //Wall jump has priority over regular jump if player is wall sliding and presses jump
        {
            isJumping = true;
            isWallJumping = true;
            wallJumpTimer = wallJumpLockTime;

            rb.linearVelocity = new Vector2(-transform.localScale.x * wallJumpForceX, wallJumpForceY);

            jumpBufferTimer = 0;
        } 
        else if (coyoteTimer > 0 && jumpBufferTimer > 0) // Regular jump if within coyote time and jump buffer time
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;

            jumpBufferTimer = 0;          
        }
        else if (jumpBufferTimer > 0 && canDoubleJump && !isWallSliding) // Double jump if jump buffer is active, double jump is available, and player is not wall sliding
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;

            canDoubleJump = false; // Consume double jump
            jumpBufferTimer = 0;
        }

        //Detect Landing
        if (!isGrounded)
        {
            wasInAir = true;

            if (transform.position.y > highestY)
            {
                highestY = transform.position.y;
            }
        }
        if (isGrounded && wasInAir)
        {
            float fallDistance = highestY - transform.position.y;
            if (fallDistance > 2f) // Adjust this threshold as needed
            {
                isLanding = true;
                animator.SetTrigger("Land");
            }
            wasInAir = false;
            highestY = transform.position.y; //reset highestY for the next jump
        }

        #endregion

        updateAnimations(moveInput, isRunning, isWallSliding);
    }

    void updateAnimations(float moveInput, bool isRunning, bool isWallSliding)
    {
        float spd = Mathf.Abs(rb.linearVelocity.x);

        animator.SetFloat("Speed", spd);
        animator.SetBool("isRunning", isRunning && spd > 0.2f);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isWallSliding", isWallSliding);

        if (isJumping)
        {
            animator.SetBool("isWallSliding", false); // Ensure wall sliding animation is not active during jump)
            animator.SetTrigger("isJumping");
            isJumping = false;
        }
    }

    public void endLanding()
    {
        isLanding = false;
    }
}
