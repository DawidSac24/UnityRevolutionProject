using _Project.Scripts.Player.State;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovementConfig))]
public class PlayerStateMachine : MonoBehaviour
{
    // [Header("Movement")]
    // [SerializeField] private float horizontalVelocity; // expose variable for debug
    // [SerializeField] private float targetMoveSpeed = 0.0f;
    //
    // [Header("Jumping")]
    // [SerializeField] private float jumpForce = 5.0f;
    // [SerializeField] private float airMultiplier = 0.4f;
    //
    // [Header("Dash")]


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
    public RewindState RewindState { get; private set; }

    public PlayerState CurrentState;

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
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        // _isGrounded = IsGrounded();

        // HandleJump();
        // HandleSlide();
        // HandleDash();
        // HandleMovement(inputHandler.MoveInput);
    }

    // private void HandleMovement(Vector2 moveVector)
    // {
    //     if (rb.isKinematic) return;
    //     Vector3 forward = transform.TransformDirection(Vector3.forward);
    //     Vector3 right = transform.TransformDirection(Vector3.right);
    //
    //     targetMoveSpeed = walkSpeed;
    //
    //     //  Weaken the input force if in the air
    //     float currentMultiplier = _isGrounded ? 1f : airMultiplier;
    //     Vector3 moveForce = (forward * moveVector.y + right * moveVector.x).normalized *
    //                         (targetMoveSpeed * currentMultiplier);
    //
    //     Vector3 currentHorizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    //     if (moveVector == Vector2.zero)
    //     {
    //         // Only apply idle friction if standing on the ground
    //         if (_isGrounded)
    //         {
    //             var currentFrition = _isSliding ? slidingFriction : idleFriction;
    //             currentHorizontalVelocity =
    //                 Vector3.Lerp(currentHorizontalVelocity, Vector3.zero, currentFrition * Time.fixedDeltaTime);
    //
    //             rb.linearVelocity = new Vector3(currentHorizontalVelocity.x, rb.linearVelocity.y,
    //                 currentHorizontalVelocity.z);
    //         }
    //     }
    //     else // The Player gives inputs - Moving
    //     {
    //         // Calculate the dot product to see if the Player is fighting its own momentum
    //         // (dot product = check if 2 vectors have the same direction, 1 = same direction, 0 = 90 degrees, -1 = 180 degrees)
    //         float dot = Vector3.Dot(moveForce.normalized, currentHorizontalVelocity.normalized);
    //
    //         // If on the ground and pushing backwards/diagonally against momentum
    //         if (_isGrounded && dot < 0)
    //         {
    //             currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero,
    //                 turnaroundFriction * Time.fixedDeltaTime);
    //             rb.linearVelocity = new Vector3(currentHorizontalVelocity.x, rb.linearVelocity.y,
    //                 currentHorizontalVelocity.z);
    //         }
    //
    //         // OVER THE LIMIT
    //         if (currentHorizontalVelocity.magnitude > targetMoveSpeed)
    //         {
    //             // Only allow AddForce if dot <= 0 (steering/braking). 
    //             if (dot <= 0)
    //             {
    //                 rb.AddForce(moveForce, ForceMode.Force);
    //             }
    //
    //             if (_isGrounded && !_isSliding)
    //             {
    //                 Vector3 targetBleedVelocity = currentHorizontalVelocity.normalized * targetMoveSpeed;
    //                 Vector3 bledVelocity = Vector3.Lerp(currentHorizontalVelocity, targetBleedVelocity,
    //                     bleedingFriction * Time.fixedDeltaTime);
    //                 rb.linearVelocity = new Vector3(bledVelocity.x, rb.linearVelocity.y, bledVelocity.z);
    //             }
    //         }
    //         // UNDER THE LIMIT
    //         else
    //         {
    //             rb.AddForce(moveForce, ForceMode.Force);
    //
    //             if (_isGrounded)
    //             {
    //                 Vector3 newHorizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    //                 if (newHorizontalVelocity.magnitude > targetMoveSpeed)
    //                 {
    //                     Vector3 hardCap = newHorizontalVelocity.normalized * targetMoveSpeed;
    //                     rb.linearVelocity = new Vector3(hardCap.x, rb.linearVelocity.y, hardCap.z);
    //                 }
    //             }
    //         }
    //
    //         horizontalVelocity = currentHorizontalVelocity.magnitude;
    //     }
    // }
    //
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