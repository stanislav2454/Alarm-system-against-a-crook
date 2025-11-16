using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private MeleeWeapon currentWeapon;

    private Userinput input;

    private void Awake()
    {
        input = GetComponent<Userinput>();
    }

    private void Update()
    {
        if (input.AttackInput && currentWeapon != null)
        {
            currentWeapon.Attack();
        }
    }

    public void EquipWeapon(MeleeWeapon weapon)
    {
        currentWeapon = weapon;
        Debug.Log($"Equipped: {weapon.Name}");
    }
}