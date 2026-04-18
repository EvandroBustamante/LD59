using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    [Tooltip("Grace period when the player can still jump after jumping off a cliff")] public float hangTime = 0.2f;
    public float cameraAheadAmount = 5f;

    [Header("Script References")]
    public Transform groundCheck1;
    public Transform groundCheck2;
    public LayerMask groundLayer;
    public Transform cameraTarget;

    private bool canMove = true;
    private bool isGrounded = false;
    private float hangTimer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private InputManager inputManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        if (!canMove) return;

        //Horizontal move:
        rb.linearVelocity = new Vector2(inputManager.moveInput.x * moveSpeed, rb.linearVelocity.y);

        //Check for ground:
        isGrounded = Physics2D.OverlapCircle(groundCheck1.position, .1f, groundLayer) || Physics2D.OverlapCircle(groundCheck2.position, .1f, groundLayer);

        if (isGrounded)
        {
            hangTimer = hangTime;
        }
        else
        {
            hangTimer -= Time.deltaTime;
        }

        //Jump:
        if (inputManager.isJumping && hangTimer > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        }
        else if (!inputManager.isJumping && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * .5f);
        }

        //Flip sprite:
        if (inputManager.moveInput.x != 0)
        {
            if (inputManager.moveInput.x > 0)
            {
                sr.flipX = false;
            }
            else if (inputManager.moveInput.x < 0)
            {
                sr.flipX = true;
            }
        }

        //Move camera target:
        if (inputManager.moveInput.x != 0)
        {
            cameraTarget.localPosition = new Vector3((cameraAheadAmount * inputManager.moveInput.x), cameraTarget.localPosition.y, cameraTarget.localPosition.z);
        }
    }
}
