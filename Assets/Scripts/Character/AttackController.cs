using UnityEngine;

[RequireComponent(typeof(Userinput), typeof(WeaponInventory))]
public class AttackController : MonoBehaviour
{
    private Userinput _input;
    private WeaponInventory _weaponInventory;

    private void Awake()
    {
        _input = GetComponent<Userinput>();
        _weaponInventory = GetComponent<WeaponInventory>();
    }

    private void Update()
    {
        HandleWeaponInput();
        HandleWeaponSwitching();
    }

    private void HandleWeaponInput()
    {
        if (_weaponInventory.CurrentWeapon == null)
            return;

        if (_input.AttackInput && _weaponInventory.CurrentWeapon.CanAttack())
            _weaponInventory.CurrentWeapon.Attack();

        if (_input.IsReloading && _weaponInventory.CurrentWeapon is RangeWeapon rangeWeapon)
            rangeWeapon.StartReload();
    }

    private void HandleWeaponSwitching()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");//TODO:

        if (scroll > 0)
        {
            _weaponInventory.SwitchToNextWeapon();
        }
        else if (scroll < 0)
        {
            _weaponInventory.SwitchToPreviousWeapon();
        }

        // Цифровые клавиши 1-9
        for (int i = 0; i < Mathf.Min(9, _weaponInventory.WeaponCount); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))//TODO:
            {
                _weaponInventory.EquipWeapon(i);
            }
        }
    }
}