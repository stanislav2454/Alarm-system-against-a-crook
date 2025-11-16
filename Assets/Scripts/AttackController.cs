using UnityEngine;

[RequireComponent(typeof(Userinput))]
public class AttackController : MonoBehaviour
{
    [SerializeField] private WeaponBase currentWeapon;

    private Userinput input;

    private void Awake()
    {
        input = GetComponent<Userinput>();
    }

    private void Update()
    {
        HandleWeaponInput();
    }

    public WeaponBase GetCurrentWeapon() => 
        currentWeapon;

    public void EquipWeapon(MeleeWeapon weapon)
    {
        currentWeapon = weapon;
        Debug.Log($"Equipped: {weapon.Name}");
    }

    private void HandleWeaponInput()
    {
        if (currentWeapon == null)
            return;

        if (input.AttackInput && currentWeapon.CanAttack())
            currentWeapon.Attack();

        if (input.IsReloading && currentWeapon is RangeWeapon rangeWeapon)
            rangeWeapon.StartReload();
    }
}