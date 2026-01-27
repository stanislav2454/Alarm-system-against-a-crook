using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private List<WeaponBase> _weapons = new List<WeaponBase>();
    [SerializeField] private int _currentWeaponIndex = 0;

    public System.Action<WeaponBase> WeaponChanged;

    public WeaponBase CurrentWeapon { get; private set; }
    public int WeaponCount => _weapons.Count;

    private void Start()
    {
        foreach (var weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        if (_weapons.Count > 0)
        {
            EquipWeapon(_currentWeaponIndex);
        }
    }

    public void AddWeapon(WeaponBase weapon)
    {
        _weapons.Add(weapon);
        weapon.gameObject.SetActive(false);

        if (CurrentWeapon == null)
        {
            EquipWeapon(_weapons.Count - 1);//TODO:
        }
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weapons.Count)
            return;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }

        _currentWeaponIndex = index;
        CurrentWeapon = _weapons[_currentWeaponIndex];
        CurrentWeapon.gameObject.SetActive(true);

        WeaponChanged?.Invoke(CurrentWeapon);
       // Debug.Log($"Equipped: {CurrentWeapon.Name}");
    }

    public void SwitchToNextWeapon()
    {
        if (_weapons.Count <= 1)
            return;

        int nextIndex = (_currentWeaponIndex + 1) % _weapons.Count;//TODO:
        EquipWeapon(nextIndex);
    }

    public void SwitchToPreviousWeapon()
    {
        if (_weapons.Count <= 1)
            return;

        int prevIndex = (_currentWeaponIndex - 1 + _weapons.Count) % _weapons.Count;//TODO:
        EquipWeapon(prevIndex);
    }
    //==================================
    public IReadOnlyList<WeaponBase> Weapons => _weapons.AsReadOnly();
    public List<WeaponBase> GetAllWeapons() =>
        new List<WeaponBase>(_weapons);
}