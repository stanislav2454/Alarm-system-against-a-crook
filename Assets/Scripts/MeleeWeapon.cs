using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private LayerMask attackMask = ~0;
    [SerializeField] private float attackAngle = 90f;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private AudioClip attackSound;

    private AudioSource audioSource;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        PlayAttackEffects();
        CheckHit();
        //Debug.Log($"Melee attack with {weaponName}");

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, attackMask))
        //{
        //    var damageable = hit.collider.GetComponent<IDamageable>();
        //    if (damageable != null)
        //    {
        //        damageable.TakeDamage(damage);
        //        Debug.Log($"Hit: {hit.collider.name} for {damage} damage");
        //    }
        //}

        ResetAttackTimer();
    }

    private void PlayAttackEffects()
    {
        if (attackEffect != null)
            attackEffect.Play();

        if (attackSound != null && audioSource != null)
            audioSource.PlayOneShot(attackSound);
    }

    private void CheckHit()
    {
        // SphereCast для площади атаки
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, attackMask);

        foreach (var hitCollider in hitColliders)
        {
            // Проверяем угол атаки
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= attackAngle / 2)
            {
                var damageable = hitCollider.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация области атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Визуализация угла атаки
        Gizmos.color = Color.yellow;
        Vector3 leftDirection = Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, leftDirection * attackRange);
        Gizmos.DrawRay(transform.position, rightDirection * attackRange);
    }
}