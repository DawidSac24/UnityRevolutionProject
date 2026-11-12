using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Arsenal")]
    [SerializeField] private GameObject[] weapons; 
    
    private PlayerInputHandler _inputHandler;
    
    private int _currentWeaponIndex = 0;

    private void Start()
    {
        EquipWeapon(_currentWeaponIndex);
    }

    private void OnEnable()
    {
        // Subscribe to the input events
        PlayerInputHandler.OnWeaponScrolled += HandleScroll;
        PlayerInputHandler.OnWeaponSlotSelected += EquipWeapon;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        PlayerInputHandler.OnWeaponScrolled -= HandleScroll;
        PlayerInputHandler.OnWeaponSlotSelected -= EquipWeapon;
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

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        _currentWeaponIndex = index;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].SetActive(i == _currentWeaponIndex);
            }
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
}