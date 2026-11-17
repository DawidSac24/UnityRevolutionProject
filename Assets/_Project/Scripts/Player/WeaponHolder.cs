using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform _playerCameraTransform;

    [Header("Arsenal")]
    [SerializeField] private GameObject[] weapons;

    private PlayerInputHandler _inputHandler;

    private int _currentWeaponIndex = 0;
    private Weapon _activeWeaponScript;

    private void Start()
    {
        EquipWeapon(_currentWeaponIndex);
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        _currentWeaponIndex = index;

        for (int i = 0; i < weapons.Length; i++)
        {
            bool isActive = (i == _currentWeaponIndex);
            weapons[i].SetActive(isActive);

            if (isActive)
            {
                _activeWeaponScript = weapons[i].GetComponent<Weapon>();
            }
        }
    }

    private void HandleShoot()
    {
        _activeWeaponScript.TryFire(_playerCameraTransform);
    }

    private void HandleScroll(float scrollY)
    {
        // Unity's scroll Y is usually positive for up, negative for down.
        // The values can be large (e.g., 120 or -120), so we just check > 0 or < 0
        if (scrollY > 0f)
        {
            SwitchWeaponUp();
        }
        else if (scrollY < 0f)
        {
            SwitchWeaponDown();
        }
    }

    private void SwitchWeaponUp()
    {
        int newIndex = _currentWeaponIndex + 1;
        if (newIndex >= weapons.Length) newIndex = 0;
        EquipWeapon(newIndex);
    }

    private void SwitchWeaponDown()
    {
        int newIndex = _currentWeaponIndex - 1;
        if (newIndex < 0) newIndex = weapons.Length - 1;
        EquipWeapon(newIndex);
    }


    private void OnEnable()
    {
        PlayerInputHandler.OnShootPressed += HandleShoot;
        PlayerInputHandler.OnWeaponScrolled += HandleScroll;
        PlayerInputHandler.OnWeaponSlotSelected += EquipWeapon;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnShootPressed -= HandleShoot;
        PlayerInputHandler.OnWeaponScrolled -= HandleScroll;
        PlayerInputHandler.OnWeaponSlotSelected -= EquipWeapon;
    }
}