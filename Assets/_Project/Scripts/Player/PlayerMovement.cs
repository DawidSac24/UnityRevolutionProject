using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(PlayerInputHandler))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalVelocity;
    [SerializeField] private float targetMoveSpeed = 0.0f;
    [SerializeField] private float walkSpeed = 15.0f;

    [Header("Frictions")]
    [SerializeField] private float idleFriction = 20.0f;
    [SerializeField] private float bleedingFriction = 3.0f;
    [SerializeField] private float turnaroundFriction = 25.0f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 20.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float airMultiplier = 0.4f;

    [Header("GroundChecks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    // private Collider _collider;
    private Rigidbody _rb;
    private PlayerInputHandler _inputHandler;

    private bool _isJumpTriggered = false;
    private bool _isDashTriggered = false;

    void Start()
    {
        // _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();

        HandleJump(isGrounded);
        HandleDash();
        HandleMovement(_inputHandler.MoveInput, isGrounded);
    }

    private void HandleMovement(Vector2 moveVector, bool isGrounded)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        targetMoveSpeed = walkSpeed;

        //  Weaken the input force if in the air
        float currentMultiplier = isGrounded ? 1f : airMultiplier;
        Vector3 moveForce = (forward * moveVector.y + right * moveVector.x).normalized *
                            (targetMoveSpeed * currentMultiplier);

        Vector3 currentHorizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        if (moveVector == Vector2.zero)
        {
            // Only apply idle friction if standing on the ground
            if (isGrounded)
            {
                currentHorizontalVelocity =
                    Vector3.Lerp(currentHorizontalVelocity, Vector3.zero, idleFriction * Time.fixedDeltaTime);

                _rb.linearVelocity = new Vector3(currentHorizontalVelocity.x, _rb.linearVelocity.y,
                    currentHorizontalVelocity.z);
            }
        }
        else // The Player gives inputs - Moving
        {
            // Calculate the dot product to see if the Player is fighting its own momentum
            // (dot product = check if 2 vectors have the same direction, 1 = same direction, 0 = 90 degrees, -1 = 180 degrees)
            float dot = Vector3.Dot(moveForce.normalized, currentHorizontalVelocity.normalized);

            // If on the ground and pushing backwards/diagonally against momentum
            if (isGrounded && dot < 0)
            {
                currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero,
                    turnaroundFriction * Time.fixedDeltaTime);
                _rb.linearVelocity = new Vector3(currentHorizontalVelocity.x, _rb.linearVelocity.y,
                    currentHorizontalVelocity.z);
            }

            // OVER THE LIMIT
            if (currentHorizontalVelocity.magnitude > targetMoveSpeed)
            {
                // Only allow AddForce if dot <= 0 (steering/braking). 
                if (dot <= 0)
                {
                    _rb.AddForce(moveForce, ForceMode.Force);
                }

                if (isGrounded)
                {
                    Vector3 targetBleedVelocity = currentHorizontalVelocity.normalized * targetMoveSpeed;
                    Vector3 bledVelocity = Vector3.Lerp(currentHorizontalVelocity, targetBleedVelocity,
                        bleedingFriction * Time.fixedDeltaTime);
                    _rb.linearVelocity = new Vector3(bledVelocity.x, _rb.linearVelocity.y, bledVelocity.z);
                }
            }
            // UNDER THE LIMIT
            else
            {
                _rb.AddForce(moveForce, ForceMode.Force);

                if (isGrounded)
                {
                    Vector3 newHorizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
                    if (newHorizontalVelocity.magnitude > targetMoveSpeed)
                    {
                        Vector3 hardCap = newHorizontalVelocity.normalized * targetMoveSpeed;
                        _rb.linearVelocity = new Vector3(hardCap.x, _rb.linearVelocity.y, hardCap.z);
                    }
                }
            }

            horizontalVelocity = currentHorizontalVelocity.magnitude;
        }
    }

    private void HandleJump(bool isGrounded)
    {
        if (!_isJumpTriggered) return;

        if (isGrounded)
        {
            Vector3 clearedHorizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.linearVelocity = clearedHorizontalVelocity;

            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Instantly consume the jump request so it doesn't get stuck true
        _isJumpTriggered = false;
    }

    private void HandleDash()
    {
        if (!_isDashTriggered) return;

        Vector3 direction = _rb.linearVelocity.normalized;
        if (direction.Equals(Vector3.zero))
            direction = Vector3.forward;
        _rb.AddForce(direction * dashForce, ForceMode.Impulse);

        _isDashTriggered = false;
    }

    private void TriggerJump()
    {
        _isJumpTriggered = true;
    }

    private void TriggerDash()
    {
        _isDashTriggered = true;
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnJumpPressed += TriggerJump;
        PlayerInputHandler.OnDashPressed += TriggerDash;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnJumpPressed -= TriggerJump;
        PlayerInputHandler.OnDashPressed -= TriggerDash;
    }
}