using _Project.Scripts.Player.State;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovementConfig))]
public class PlayerStateMachine : MonoBehaviour
{
    // private bool _isGrounded = false;
    // private bool _isJumpTriggered = false;
    //
    // private bool _isSliding = false;
    // private float _lastSlideTime = 0.0f;
    //
    // private bool _isDashTriggered = false;

    // // Store original collider stats to restore them after sliding
    // private float _originalColliderHeight;
    // private Vector3 _originalColliderCenter;

    public CapsuleCollider Collider { get; private set; }
    public Rigidbody Rb { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerMovementConfig MovementConfig { get; private set; }


    // local states
    public WalkingState WalkingState { get; private set; }
    public AirborneState AirborneState { get; private set; }
    public SlidingState SlidingState { get; private set; }
    // global states
    public DashingState DashingState { get; private set; }
    public RewindingState RewindingState { get; private set; }

    private PlayerState CurrentState;

    void Start()
    {
        Collider = GetComponent<CapsuleCollider>();
        Rb = GetComponent<Rigidbody>();
        InputHandler = GetComponent<PlayerInputHandler>();

        MovementConfig = GetComponent<PlayerMovementConfig>();

        WalkingState = new WalkingState(this);
        AirborneState = new AirborneState(this);
        SlidingState = new SlidingState(this);
        DashingState = new DashingState(this);
        // TODO: Implement RewindingState
        // RewindingState = new RewindingState(this);

        CurrentState = WalkingState;
        CurrentState.Enter();
    }

    void Update()
    {
        CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        CurrentState.PhysicsUpdate();
        // _isGrounded = IsGrounded();

        // HandleJump();
        // HandleSlide();
        // HandleDash();
        // HandleMovement(inputHandler.MoveInput);
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnJumpPressed += HandleJumpInput;
        PlayerInputHandler.OnSlidePressed += HandleSlideInput;
        PlayerInputHandler.OnDashPressed += HandleDashInput;
        // TODO: Implement RewindingState
        // PlayerInputHandler.OnRewindPressed += HandleRewindInput;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnJumpPressed -= HandleJumpInput;
        PlayerInputHandler.OnSlidePressed -= HandleSlideInput;
        PlayerInputHandler.OnDashPressed -= HandleDashInput;
        // TODO: Implement RewindingState
        // PlayerInputHandler.OnRewindPressed -= HandleRewindInput;
    }

    // Local triggers need to be overwritten by the current state (or not if we don't want any behavior
    private void HandleJumpInput() => CurrentState?.OnJumpInput();
    private void HandleSlideInput() => CurrentState?.OnSlideInput();

    // Global triggers are handled directly in the state machine
    private void HandleDashInput()
    {
        if (CurrentState != DashingState)
        {
            ChangeState(DashingState);
        }
    }

    // TODO: Implement RewindingState
    // private void HandleRewindInput()
    // {
    //     if (CurrentState != RewindingState)
    //     {
    //         ChangeState(RewindingState);
    //     }
    // }

    // private void HandleJump()
    // {
    //     if (!_isJumpTriggered) return;
    //
    //     if (_isGrounded)
    //     {
    //         Vector3 clearedHorizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    //         rb.linearVelocity = clearedHorizontalVelocity;
    //
    //         rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //     }
    //
    //     // Instantly consume the jump request so it doesn't get stuck true
    //     _isJumpTriggered = false;
    // }
    //
    // private void HandleSlide()
    // {
    //     if (!_isSliding) return;
    //
    //     // Stop sliding if the button is released AND the minimum slide duration has passed
    //     if ((!inputHandler.SlideInput && (Time.time - _lastSlideTime > slideDuration)) ||
    //         horizontalVelocity < walkSpeed)
    //     {
    //         StopSlide();
    //         return;
    //     }
    // }
    //
    // private void StartSlide()
    // {
    //     if (_isSliding) return;
    //
    //     foreach (var t in rotateOnSlideTransforms)
    //     {
    //         float targetXRotation = t.localEulerAngles.x + slideRotationXOffset;
    //         t.localRotation = Quaternion.Euler(targetXRotation, t.localEulerAngles.y, t.localEulerAngles.z);
    //     }
    //
    //     foreach (var t in moveOnSlideTransforms)
    //     {
    //         float targetYPosition = t.localPosition.y + slidePositionYOffset;
    //         t.localPosition = new Vector3(t.localPosition.x, targetYPosition, t.localPosition.z);
    //     }
    //
    //     // Shrink the collider so the player can slide under obstacles
    //     // _collider.height = _originalColliderHeight * 0.5f; // Cut height in half
    //     // _collider.center =
    //     //     new Vector3(0, _originalColliderHeight * -0.25f, 0); // Move center down so feet stay on floor
    //
    //     _lastSlideTime = Time.time;
    //     _isSliding = true;
    // }
    //
    // private void StopSlide()
    // {
    //     foreach (var t in rotateOnSlideTransforms)
    //     {
    //         float targetXRotation = t.localEulerAngles.x - slideRotationXOffset;
    //         t.localRotation = Quaternion.Euler(targetXRotation, t.localEulerAngles.y, t.localEulerAngles.z);
    //     }
    //
    //     foreach (var t in moveOnSlideTransforms)
    //     {
    //         float targetYPosition = t.localPosition.y - slidePositionYOffset;
    //         t.localPosition = new Vector3(t.localPosition.x, targetYPosition, t.localPosition.z);
    //     }
    //
    //     // Restore original collider size
    //     // _collider.height = _originalColliderHeight;
    //     // _collider.center = _originalColliderCenter;
    //
    //     _isSliding = false;
    // }
    //
    // private void HandleDash()
    // {
    //     if (!_isDashTriggered) return;
    //
    //     Vector3 direction = rb.linearVelocity.normalized;
    //     if (direction.x == 0 || direction.z == 0)
    //         direction = Vector3.forward;
    //     rb.AddForce(new Vector3(direction.x, 0, direction.z) * dashForce, ForceMode.Impulse);
    //
    //     _isDashTriggered = false;
    // }

    // private void TriggerJump()
    // {
    //     _isJumpTriggered = true;
    // }
    //
    // private void TriggerSLide()
    // {
    //     StartSlide();
    // }
    //
    // private void TriggerDash()
    // {
    //     _isDashTriggered = true;
    // }
    //

    // private void OnEnable()
    // {
    //     PlayerInputHandler.OnJumpPressed += TriggerJump;
    //     PlayerInputHandler.OnSlidePressed += TriggerSLide;
    //     PlayerInputHandler.OnDashPressed += TriggerDash;
    // }
    //
    // private void OnDisable()
    // {
    //     PlayerInputHandler.OnJumpPressed -= TriggerJump;
    //     PlayerInputHandler.OnSlidePressed -= TriggerSLide;
    //     PlayerInputHandler.OnDashPressed -= TriggerDash;
    // }
}