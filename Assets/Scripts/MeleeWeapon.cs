using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private LayerMask attackMask = ~0;

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        Debug.Log($"Melee attack with {weaponName}");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, attackMask))
        {
            var damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Debug.Log($"Hit: {hit.collider.name} for {damage} damage");
            }
        }

        ResetAttackTimer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * attackRange);
    }
}