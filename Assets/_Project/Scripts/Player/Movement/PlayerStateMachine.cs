using _Project.Scripts.Player.State;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovementConfig), typeof(PlayerInputHandler))]
[RequireComponent(typeof(DashCharges))]
public class PlayerStateMachine : MonoBehaviour
{
    public CapsuleCollider Collider { get; private set; }
    public Rigidbody Rb { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerMovementConfig MovementConfig { get; private set; }

    // local states
    public WalkingState WalkingState { get; private set; }
    public AirborneState AirborneState { get; private set; }
    public SlidingState SlidingState { get; private set; }
    public DashingState DashingState { get; private set; }
    public DashCharges DashCharges { get; private set; }
    // global states
    public RewindingState RewindingState { get; private set; }

    public PlayerState CurrentState { get; private set; }
    public PlayerState LastState { get; private set; }

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

        DashCharges = GetComponent<DashCharges>();

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
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState?.Exit();
        LastState = CurrentState;
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
    private void HandleDashInput() => CurrentState?.OnDashInput();

    // Global triggers are handled directly in the state machine
    // private void HandleDashInput()
    // {
    //     if (CurrentState != DashingState)
    //     {
    //         ChangeState(DashingState);
    //     }
    // }

    // TODO: Implement RewindingState
    // private void HandleRewindInput()
    // {
    //     if (CurrentState != RewindingState)
    //     {
    //         ChangeState(RewindingState);
    //     }
    // }
}