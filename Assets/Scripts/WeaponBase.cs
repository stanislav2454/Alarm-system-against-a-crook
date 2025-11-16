using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [Header("Basic Weapon Settings")]
    [SerializeField] protected string WeaponName = "Weapon";
    [SerializeField] protected float AttackRate = 1f;

    protected float NextAttackTime;

    public string Name => WeaponName;

    public virtual bool CanAttack()
    {
        return Time.time >= NextAttackTime;
    }

    public abstract void Attack();

    protected void ResetAttackTimer()
    {
        NextAttackTime = Time.time + AttackRate;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 5);
    }
}