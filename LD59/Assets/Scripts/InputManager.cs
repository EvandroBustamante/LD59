using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public bool isRestarting = false;
    [HideInInspector] public bool isPausing = false;

    PlayerInputs playerInputs;

    private void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Player.Enable();
        playerInputs.Player.Pause.performed += OnPausePerformed;
        playerInputs.Player.Pause.canceled += OnPauseCanceled;
        playerInputs.Player.Restart.performed += OnRestartPerformed;
        playerInputs.Player.Jump.performed += OnJumpPerformed;
        playerInputs.Player.Jump.canceled += OnJumpCanceled;
        playerInputs.Player.Dash.performed += OnDashPerformed;
        playerInputs.Player.Dash.canceled += OnDashCanceled;
        playerInputs.Player.Interact.performed += OnInteractPerformed;
        playerInputs.Player.Interact.canceled += OnInteractCanceled;
    }

    private void OnDestroy()
    {
        playerInputs.Player.Pause.performed -= OnPausePerformed;
        playerInputs.Player.Pause.canceled -= OnPauseCanceled;
        playerInputs.Player.Restart.performed -= OnRestartPerformed;
        playerInputs.Player.Jump.performed -= OnJumpPerformed;
        playerInputs.Player.Jump.canceled -= OnJumpCanceled;
        playerInputs.Player.Dash.performed -= OnDashPerformed;
        playerInputs.Player.Dash.canceled -= OnDashCanceled;
        playerInputs.Player.Interact.performed -= OnInteractPerformed;
        playerInputs.Player.Interact.canceled -= OnInteractCanceled;
        playerInputs.Player.Disable();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        isJumping = true;
    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        isJumping = false;
    }

    void OnDashPerformed(InputAction.CallbackContext context)
    {
        isDashing = true;
    }

    void OnDashCanceled(InputAction.CallbackContext context)
    {
        isDashing = false;
    }

    void OnInteractPerformed(InputAction.CallbackContext context)
    {
        isInteracting = true;
    }

    void OnInteractCanceled(InputAction.CallbackContext context)
    {
        isInteracting = false;
    }

    void OnRestartPerformed(InputAction.CallbackContext context)
    {
        isRestarting = true;
    }

    void OnPausePerformed(InputAction.CallbackContext context)
    {
        isPausing = true;
    }

    void OnPauseCanceled(InputAction.CallbackContext context)
    {
        isPausing = false;
    }
}
