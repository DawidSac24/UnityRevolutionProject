using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private const string ACTION_MAP = "Player";
    // Movement
    private const string MOVE_ACTION = "Move";
    private const string LOOK_ACTION = "Look";
    private const string JUMP_ACTION = "Jump";
    private const string DASH_ACTION = "Dash";
    private const string REWIND_ACTION = "Rewind";

    // Weapons / Shooting
    private const string SHOOT_ACTION = "Attack";
    private const string WEAPON_SCROLL_ACTION = "WeaponScroll";
    private const string WEAPON_SLOT_1_ACTION = "EquipSlot1";
    private const string WEAPON_SLOT_2_ACTION = "EquipSlot2";

    [SerializeField] private InputActionAsset inputActions;
    // Movement
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _rewindAction;

    // Weapons / Shooting
    private InputAction _shootAction;
    private InputAction _weaponScrollAction;
    private InputAction _weaponSlot1Action;
    private InputAction _weaponSlot2Action;

    // INPUTS
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    // ACTIONS
    // Movement
    public static event Action OnJumpPressed;
    public static event Action OnDashPressed;
    public static event Action OnRewindPressed;

    // Weapons / Shooting
    public static event Action OnShootPressed;
    public static event Action<float> OnWeaponScrolled;
    public static event Action<int> OnWeaponSlotSelected;

    private void Awake()
    {
        // Movement
        _moveAction = inputActions.FindActionMap(ACTION_MAP).FindAction(MOVE_ACTION);
        _lookAction = inputActions.FindActionMap(ACTION_MAP).FindAction(LOOK_ACTION);
        _jumpAction = inputActions.FindActionMap(ACTION_MAP).FindAction(JUMP_ACTION);
        _dashAction = inputActions.FindActionMap(ACTION_MAP).FindAction(DASH_ACTION);
        _rewindAction = inputActions.FindActionMap(ACTION_MAP).FindAction(REWIND_ACTION);

        // Weapons / Shooting
        _shootAction = inputActions.FindActionMap(ACTION_MAP).FindAction(SHOOT_ACTION);
        _weaponScrollAction = inputActions.FindActionMap(ACTION_MAP).FindAction(WEAPON_SCROLL_ACTION);
        _weaponSlot1Action = inputActions.FindActionMap(ACTION_MAP).FindAction(WEAPON_SLOT_1_ACTION);
        _weaponSlot2Action = inputActions.FindActionMap(ACTION_MAP).FindAction(WEAPON_SLOT_2_ACTION);
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
        // Movement
        MoveInput = _moveAction.ReadValue<Vector2>();
        LookInput = _lookAction.ReadValue<Vector2>();

        if (_jumpAction.triggered) OnJumpPressed?.Invoke();
        if (_dashAction.triggered) OnDashPressed?.Invoke();
        if (_rewindAction.triggered) OnRewindPressed?.Invoke();

        // Weapons / Shooting
        if (_shootAction.triggered) OnShootPressed?.Invoke();
        if (_weaponScrollAction.triggered)
        {
            float scrollValue = _weaponScrollAction.ReadValue<Vector2>().y;
            OnWeaponScrolled?.Invoke(scrollValue);
        }

        if (_weaponSlot1Action.triggered) OnWeaponSlotSelected?.Invoke(0);
        if (_weaponSlot2Action.triggered) OnWeaponSlotSelected?.Invoke(1);
    }
}