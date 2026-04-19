using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public float doubleJumpHeight = 3f;
    public float doubleJumpBuffer = 0.2f;
    public float weakDashForce = 2f;
    public float strongDashForce = 4f;
    public float weakDashDuration = 0.1f;
    public float strongDashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float timeToDie = 3;
    [Tooltip("Grace period when the player can still jump after jumping off a cliff")] public float hangTime = 0.2f;
    public float cameraAheadAmount = 5f;

    [Header("Script References")]
    public Transform groundCheck1;
    public Transform groundCheck2;
    public LayerMask groundLayer;
    public Transform cameraTarget;
    public CameraFollow cameraFollow;

    private bool canMove = true;
    private bool isFacingRight = true;
    [HideInInspector] public SignalType currentSignal = SignalType.NoSignal;

    private bool isGrounded = false;
    private bool canDoubleJump = true;
    private float hangTimer;
    private bool hasJumped = false;
    private bool hasDoubleJumped = false;
    private float doubleJumpBufferTimer;
    private bool instantiatedJumpVFX = false;
    private bool animatingJump = false;

    private bool hasWeakDash = false;
    private bool hasStrongDash = false;
    private bool isDashing = false;
    private float dashCooldownTimer;
    private bool dashReset = false;

    private bool canInteract = false;
    private BoostSignalInteractable interactableRef;

    private bool inDeathTimer = false;
    [HideInInspector] public float dieTimer = 0f;

    private Chunk currentChunk;
    private Transform respawnPoint;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private InputManager inputManager;
    private Animator animator;

    [Header("VFX")]
    public ParticleSystem jumpVFX;
    public ParticleSystem runVFX;
    public float runVFXinterval;

    private float runVFXtimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();

        cameraFollow.followTarget = cameraTarget;
    }

    private void Update()
    {
        SignalLogic();
        InteractLogic();
        RunVFXLogic();
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        if (!canMove || isDashing) return;

        //Horizontal move:
        rb.linearVelocity = new Vector2(inputManager.moveInput.x * moveSpeed, rb.linearVelocity.y);

        //Check for ground:
        isGrounded = Physics2D.OverlapCircle(groundCheck1.position, .02f, groundLayer) || Physics2D.OverlapCircle(groundCheck2.position, .1f, groundLayer);

        if (isGrounded)
        {
            hangTimer = hangTime;

            hasJumped = false;
            hasDoubleJumped = false;
            doubleJumpBufferTimer = doubleJumpBuffer;
            instantiatedJumpVFX = false;

            dashReset = true;

            animator.SetBool("hitGround", true);
            animator.SetBool("isDoubleJumping", false);
        }
        else
        {
            hangTimer -= Time.deltaTime;
            doubleJumpBufferTimer -= Time.deltaTime;

            animator.SetBool("hitGround", false);
        }

        //Jump:
        if (inputManager.isJumping && hangTimer > 0 && !hasJumped)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);

            if (!instantiatedJumpVFX)
            {
                GameObject newParticle = Instantiate(jumpVFX.gameObject, groundCheck1.transform.position, Quaternion.identity);
                Destroy(newParticle, 5f);
                instantiatedJumpVFX = true;
            }
        }
        else if (!inputManager.isJumping && rb.linearVelocity.y > 0 && !isGrounded)
        {
            hasJumped = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * .5f);
        }

        if(!isGrounded && canDoubleJump && inputManager.isJumping && hasJumped && !hasDoubleJumped && doubleJumpBufferTimer < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpHeight);
            hasDoubleJumped = true;

            GameObject newParticle = Instantiate(jumpVFX.gameObject, groundCheck1.transform.position, Quaternion.identity);
            Destroy(newParticle, 5f);

            animator.SetBool("isDoubleJumping", true);
        }

        //Dash:
        if (hasWeakDash || hasStrongDash)
        {
            if (inputManager.isDashing && dashCooldownTimer < 0 && !isDashing && dashReset)
            {
                StartCoroutine(Dash());
            }

            //cooldown:
            dashCooldownTimer -= Time.deltaTime;
        }

        //Flip sprite + animation:
        if (inputManager.moveInput.x != 0)
        {
            animator.SetBool("isRunning", true);
            if (inputManager.moveInput.x > 0)
            {
                sr.flipX = false;
                isFacingRight = true;
            }
            else if (inputManager.moveInput.x < 0)
            {
                sr.flipX = true;
                isFacingRight = false;
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (rb.linearVelocity.y != 0)
        {
            animator.SetFloat("jumpVel", rb.linearVelocity.y);
            animatingJump = true;
        }

        //Move camera target:
        if (inputManager.moveInput.x != 0)
        {
            cameraTarget.localPosition = new Vector3((cameraAheadAmount * inputManager.moveInput.x), cameraTarget.localPosition.y, cameraTarget.localPosition.z);
        }
    }

    private void SignalLogic()
    {
        switch (currentSignal)
        {
            case SignalType.NoSignal:
                canDoubleJump = false;
                hasWeakDash = false;
                hasStrongDash = false;
                if(!inDeathTimer) StartCoroutine(NoSignalTimer());
                break;
            case SignalType.WeakSignal:
                canDoubleJump = false;
                hasWeakDash = true;
                hasStrongDash = false;
                inDeathTimer = false;
                break;
            case SignalType.StrongSignal:
                canDoubleJump = true;
                hasWeakDash = false;
                hasStrongDash = true;
                inDeathTimer = false;
                break;
        }
    }

    private void InteractLogic()
    {
        if(canInteract && interactableRef != null)
        {
            if (inputManager.isInteracting)
            {
                interactableRef.Interact();
            }
        }
    }

    private void RunVFXLogic()
    {
        if(isGrounded && inputManager.moveInput.x != 0)
        {
            runVFXtimer -= Time.deltaTime;
            if(runVFXtimer < 0)
            {
                GameObject newParticle = Instantiate(runVFX.gameObject, groundCheck1.transform.position, Quaternion.identity);
                Destroy(newParticle, 2f);
                runVFXtimer = runVFXinterval;
            }
        }
        else
        {
            runVFXtimer = runVFXinterval;
        }
    }

    private IEnumerator Dash()
    {
        float multiplier = 1;
        if (!isFacingRight) multiplier = -1;

        float previousGravity = rb.gravityScale;
        rb.gravityScale = 0;

        float dashDuration = 0;

        if (hasStrongDash)
        {
            rb.linearVelocity = new Vector2(strongDashForce * multiplier, 0);
            dashDuration = strongDashDuration;
        }
        else if (hasWeakDash)
        {
            rb.linearVelocity = new Vector2(weakDashForce * multiplier, 0);
            dashDuration = weakDashDuration;
        }

        animator.SetBool("isDashing", true);

        isDashing = true;
        dashReset = false;

        yield return new WaitForSeconds(dashDuration);

        animator.SetBool("isDashing", false);

        isDashing = false;
        dashCooldownTimer = dashCooldown;
        rb.gravityScale = previousGravity;
    }

    private void UpdateCurrentChunk(Chunk newChunk)
    {
        currentChunk = newChunk;
        respawnPoint = newChunk.respawnPoint;
        cameraFollow.chunkBounds = newChunk.chunkBound;
    }

    private void RespawnPlayer()
    {
        transform.position = respawnPoint.transform.position;
        dieTimer = timeToDie;
        inDeathTimer = false;
    }

    private IEnumerator NoSignalTimer()
    {
        inDeathTimer = true;
        dieTimer = timeToDie;

        while (dieTimer > 0)
        {
            if(currentSignal != SignalType.NoSignal)
            {
                yield break;
            }

            dieTimer -= Time.deltaTime;
            yield return null;
        }

        RespawnPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chunk"))
        {
            UpdateCurrentChunk(collision.GetComponent<Chunk>());
        }

        if (collision.CompareTag("WeakSignal") && currentSignal == SignalType.NoSignal)
        {
            currentSignal = SignalType.WeakSignal;
        }
        else if(collision.CompareTag("StrongSignal") && currentSignal == SignalType.WeakSignal)
        {
            currentSignal = SignalType.StrongSignal;
        }

        if (collision.CompareTag("Interactable") && collision.GetComponent<BoostSignalInteractable>())
        {
            canInteract = true;
            interactableRef = collision.GetComponent<BoostSignalInteractable>();
        }

        if (collision.CompareTag("Death"))
        {
            RespawnPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("StrongSignal") && currentSignal == SignalType.StrongSignal)
        {
            currentSignal = SignalType.WeakSignal;
        }
        
        if(collision.CompareTag("WeakSignal") && currentSignal == SignalType.WeakSignal)
        {
            currentSignal = SignalType.NoSignal;
        }

        if (collision.CompareTag("Interactable"))
        {
            canInteract = false;
        }
    }
}

public enum SignalType
{
    NoSignal,
    WeakSignal,
    StrongSignal
}
