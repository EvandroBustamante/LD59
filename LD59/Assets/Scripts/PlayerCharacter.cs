using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public float jumpDuration = 1f;
    public float doubleJumpHeight = 3f;
    public float doubleJumpDuration = 0.15f;
    public float doubleJumpBuffer = 0.2f;
    public float weakDashForce = 2f;
    public float strongDashForce = 4f;
    public float weakDashDuration = 0.1f;
    public float strongDashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float timeToDie = 3;
    [Tooltip("Grace period when the player can still jump after jumping off a cliff")] public float hangTime = 0.2f;
    public float cameraAheadAmount = 5f;
    public float cameraAheadSpeed = 1f;
    public float deathAnimationJumpPower = 2f;
    public float deathAnimationDuration = 3f;

    [Header("Script References")]
    public Transform groundCheck1;
    public Transform groundCheck2;
    public Transform headCheck;
    public LayerMask groundLayer;
    public Transform cameraTarget;
    public CameraFollow cameraFollow;

    private bool canMove = true;
    private bool isFacingRight = true;
    [HideInInspector] public SignalType currentSignal = SignalType.NoSignal;

    private bool isGrounded = false;
    private bool canDoubleJump = true;
    private float hangTimer;
    private bool isJumping = false;
    private float jumpTimer = 0;
    private float doubleJumpTimer = 0;
    private bool isDoubleJumping = false;
    private float doubleJumpBufferTimer;
    private bool pressedJumpKey = false;

    private bool hasWeakDash = false;
    private bool hasStrongDash = false;
    private bool isDashing = false;
    private float dashCooldownTimer;
    private bool dashReset = false;
    private ShadowTrail shadowTrail;

    private bool isInWeakSignal = false;
    private bool isInStrongSignal = false;

    private bool canInteract = false;
    private BoostSignalInteractable interactableRef;

    private bool inDeathTimer = false;
    [HideInInspector] public float dieTimer = 0f;
    private bool isRespawning = false;

    private Chunk currentChunk;
    private Transform respawnPoint;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private InputManager inputManager;
    private Animator animator;
    private Collider2D col;

    [Header("VFX")]
    public ParticleSystem jumpVFX;
    public ParticleSystem runVFX;
    public float runVFXinterval;
    public ParticleSystem deathArroba;
    public ParticleSystem deathDust;

    private float runVFXtimer;

    private void Start()
    {
        shadowTrail = GetComponent<ShadowTrail>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        cameraFollow.followTarget = cameraTarget;
        currentSignal = SignalType.NoSignal;
    }

    private void Update()
    {
        SignalLogic();
        InteractLogic();
        RunVFXLogic();
        CheckIfSquished();
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
        isGrounded = Physics2D.OverlapCircle(groundCheck1.position, .025f, groundLayer) || Physics2D.OverlapCircle(groundCheck2.position, .025f, groundLayer);

        if (isGrounded)
        {
            hangTimer = hangTime;

            doubleJumpBufferTimer = doubleJumpBuffer;

            isJumping = false;
            isDoubleJumping = false;

            dashReset = true;

            shadowTrail.Activate(false);
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
        if (inputManager.isJumping && hangTimer > 0 && !isJumping && !pressedJumpKey)
        {
            StartCoroutine(Jump());
        }

        if (!isGrounded && canDoubleJump && inputManager.isJumping && !isDoubleJumping && doubleJumpBufferTimer < 0 && !pressedJumpKey)
        {
            StartCoroutine(DoubleJump());
        }

        if (!inputManager.isJumping) pressedJumpKey = false;

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
        }

        //Move camera target:
        int facingValue = 0;
        if (isFacingRight) facingValue = 1;
        else if (!isFacingRight) facingValue = -1;
        Vector3 targetPos = new Vector3((cameraAheadAmount * facingValue), cameraTarget.localPosition.y, cameraTarget.localPosition.z);
    }

    private void SignalLogic()
    {
        if(isInWeakSignal && isInStrongSignal)
        {
            currentSignal = SignalType.StrongSignal;
        }
        else if(isInWeakSignal && !isInStrongSignal)
        {
            currentSignal = SignalType.WeakSignal;
        }
        else if (!isInWeakSignal && !isInStrongSignal)
        {
            currentSignal = SignalType.NoSignal;
        }

        switch (currentSignal)
        {
            case SignalType.NoSignal:
                canDoubleJump = false;
                hasWeakDash = false;
                hasStrongDash = false;
                if (!inDeathTimer) StartCoroutine(NoSignalTimer());
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

    private IEnumerator Jump()
    {
        isJumping = true;
        pressedJumpKey = true;

        AudioManager.Instance.PlayCharacterJump();
        GameObject newParticle = Instantiate(jumpVFX.gameObject, groundCheck1.transform.position, Quaternion.identity);
        Destroy(newParticle, 5f);

        jumpTimer = jumpDuration;

        while (inputManager.isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
            jumpTimer -= Time.deltaTime;

            if (jumpTimer < 0)
            {
                //Jump ended through duration:
                yield break;
            }

            yield return null;
        }

        if (!inputManager.isJumping)
        {
            //Jump ended through cancel input:
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * .5f);
            pressedJumpKey = false;
            yield break;
        }
    }

    private IEnumerator DoubleJump()
    {
        isDoubleJumping = true;
        pressedJumpKey = true;

        AudioManager.Instance.PlayCharacterDoubleJump();
        GameObject newParticle = Instantiate(jumpVFX.gameObject, groundCheck1.transform.position, Quaternion.identity);
        Destroy(newParticle, 5f);
        shadowTrail.Activate(true);

        animator.SetBool("isDoubleJumping", true);

        doubleJumpTimer = doubleJumpDuration;

        while (inputManager.isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpHeight);
            doubleJumpTimer -= Time.deltaTime;

            if(doubleJumpTimer < 0)
            {
                //Jump ended through duration:
                yield break;
            }

            yield return null;
        }

        if (inputManager.isJumping)
        {
            //Jump ended through cancel input:
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * .5f);
            pressedJumpKey = false;
            yield break;
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
            AudioManager.Instance.PlayCharacterDashStrong();
            rb.linearVelocity = new Vector2(strongDashForce * multiplier, 0);
            dashDuration = strongDashDuration;
        }
        else if (hasWeakDash)
        {
            AudioManager.Instance.PlayCharacterDashWeak();
            rb.linearVelocity = new Vector2(weakDashForce * multiplier, 0);
            dashDuration = weakDashDuration;
        }

        animator.SetBool("isDashing", true);

        isDashing = true;
        dashReset = false;

        shadowTrail.Activate(true);

        yield return new WaitForSeconds(dashDuration);

        shadowTrail.Activate(false);
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

    private IEnumerator RespawnPlayer()
    {
        if (isRespawning) yield break;

        animator.SetTrigger("death");
        col.enabled = false;
        DisablePlayerControls();
        cameraFollow.canFollow = false;
        isRespawning = true;
        float randomFinalX = Random.Range(transform.position.x - 3, transform.position.x + 3);
        Vector3 finalPos = new Vector3(randomFinalX, transform.position.y - 3, transform.position.z);
        transform.DOJump(finalPos, deathAnimationJumpPower, 1, deathAnimationDuration);
        GameObject arrobaParticle = Instantiate(deathArroba.gameObject, transform.position, Quaternion.identity);
        Destroy(arrobaParticle, 3f);

        GameObject dustParticle = Instantiate(deathDust.gameObject, transform.position, Quaternion.identity);
        Destroy(dustParticle, 3f);

        yield return new WaitForSeconds(3f);

        animator.SetTrigger("respawn");
        rb.linearVelocity = new Vector3(0, 0, 0);
        cameraFollow.canFollow = true;
        transform.position = respawnPoint.transform.position;
        col.enabled = true;

        yield return new WaitForSeconds(0.5f);

        EnablePlayerControls();
        isRespawning = false;
        currentSignal = SignalType.NoSignal;
        dieTimer = timeToDie;
        inDeathTimer = false;
    }

    public void EnablePlayerControls()
    {
        canMove = true;
    }

    public void DisablePlayerControls()
    {
        canMove = false;
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

        StartCoroutine(RespawnPlayer());
    }

    private void CheckIfSquished()
    {
        if (!canMove) return;

        bool groundBelow = Physics2D.OverlapCircle(groundCheck1.position, .0025f, groundLayer) || Physics2D.OverlapCircle(groundCheck2.position, .1f, groundLayer);
        bool groundAbove = Physics2D.OverlapCircle(headCheck.position, .0025f, groundLayer);

        if (groundBelow && groundAbove)
        {
            Debug.Log("squished!");
            StartCoroutine(RespawnPlayer());
        }
    }

    public void AudioSteps()
    {
        AudioManager.Instance.PlayCharacterSteps();
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chunk"))
        {
            UpdateCurrentChunk(collision.GetComponent<Chunk>());
        }

        if (collision.CompareTag("WeakSignal"))
        {
            isInWeakSignal = true;
        }

        if(collision.CompareTag("StrongSignal"))
        {
            isInStrongSignal = true;
        }

        if (collision.CompareTag("Interactable") && collision.GetComponent<BoostSignalInteractable>())
        {
            canInteract = true;
            interactableRef = collision.GetComponent<BoostSignalInteractable>();
        }

        if (collision.CompareTag("Death"))
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WeakSignal"))
        {
            isInWeakSignal = false;
        }

        if (collision.CompareTag("StrongSignal"))
        {
            isInStrongSignal = false;
        }

        if (collision.CompareTag("Interactable"))
        {
            canInteract = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("WeakSignal"))
        {
            isInWeakSignal = true;
        }

        if (collision.CompareTag("StrongSignal"))
        {
            isInStrongSignal = true;
        }

        if (collision.CompareTag("Chunk"))
        {
            UpdateCurrentChunk(collision.GetComponent<Chunk>());
        }
    }
}

public enum SignalType
{
    NoSignal,
    WeakSignal,
    StrongSignal
}
