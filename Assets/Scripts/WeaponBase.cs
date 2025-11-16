using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [Header("Basic Weapon Settings")]
    [SerializeField] protected string weaponName = "Weapon";
    [SerializeField] protected float attackRate = 1f;

    protected float nextAttackTime;

    public string Name => weaponName;

    public virtual bool CanAttack()
    {
        return Time.time >= nextAttackTime;
    }

    public abstract void Attack();

    protected void ResetAttackTimer()
    {
        nextAttackTime = Time.time + attackRate;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 5);
    }
}