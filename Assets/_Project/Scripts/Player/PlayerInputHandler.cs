using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private const string ACTION_MAP = "Player";
    private const string MOVE_ACTION = "Move";
    private const string LOOK_ACTION = "Look";
    private const string JUMP_ACTION = "Jump";
    private const string DASH_ACTION = "Dash";
    private const string REWIND_ACTION = "Rewind";

    [SerializeField] private InputActionAsset inputActions;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _rewindAction;

    // INPUTS
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    // ACTIONS
    public static event Action OnJumpPressed;
    public static event Action OnDashPressed;
    public static event Action OnRewindPressed;

    private void Awake()
    {
        _moveAction = inputActions.FindActionMap(ACTION_MAP).FindAction(MOVE_ACTION);
        _lookAction = inputActions.FindActionMap(ACTION_MAP).FindAction(LOOK_ACTION);
        _jumpAction = inputActions.FindActionMap(ACTION_MAP).FindAction(JUMP_ACTION);
        _dashAction = inputActions.FindActionMap(ACTION_MAP).FindAction(DASH_ACTION);
        _rewindAction =  inputActions.FindActionMap(ACTION_MAP).FindAction(REWIND_ACTION);
    }

    private void OnEnable()
    {
        inputActions.FindActionMap(ACTION_MAP).Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap(ACTION_MAP).Disable();
    }

    void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        LookInput = _lookAction.ReadValue<Vector2>();

        if (_jumpAction.triggered)
        {
            OnJumpPressed?.Invoke();
        }

        if (_dashAction.triggered)
        {
            OnDashPressed?.Invoke();
        }

        if (_rewindAction.triggered)
        {
            OnRewindPressed?.Invoke();
        }
    }
}