using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Melee Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _damage = 25f;
    [SerializeField] private float _attackAngle = 90f;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private LayerMask _attackMask = ~0;

    [Header("Visual Effects")]
   // [SerializeField] private ParticleSystem _attackEffect;
    [SerializeField] private AudioClip _attackSound;

    private AudioSource _audioSource;
    // private float _finalDamage;

    protected override void Awake()
    {
        base.Awake();

        _audioSource = gameObject.AddComponent<AudioSource>();

        //// Проверяем, есть ли настройки с интерфейсом IMeleeWeaponSettings
        //if (_weaponSettings != null && _weaponSettings is IMeleeWeaponSettings meleeSettings)
        //{
        //    _attackRange = meleeSettings.AttackRange;
        //    _damage = meleeSettings.BaseDamage;
        //    _attackAngle = meleeSettings.AttackAngle;
        //    _knockbackForce = meleeSettings.KnockbackForce;
        //    _attackMask = meleeSettings.AttackMask;
        //}

        //// Инициализируем урон
        //_finalDamage = _weaponSettings != null ? baseDamage : _damage;
    }

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        NotifyDecoratorsOnAttack();
        //// Обновляем урон с учетом множителей декораторов
        //_finalDamage = (_weaponSettings != null ? baseDamage : _damage) * GetTotalDamageMultiplier();
        PlayAttackEffects();
        PerformMeleeAttack();
        ResetAttackTimer();
    }

    private void PlayAttackEffects()
    {
        //_attackEffect?.Play();
        _audioSource?.PlayOneShot(_attackSound);
    }

    private void PerformMeleeAttack()
    {
        float finalDamage = (_weaponSettings != null ? baseDamage : _damage) * GetTotalDamageMultiplier();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _attackRange, _attackMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject)
                continue;

            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= _attackAngle / 2)
            {
                ApplyDamage(hitCollider, finalDamage);
                ApplyKnockback(hitCollider);
            }
        }
    }

    private void ApplyDamage(Collider target, float damage)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(damage);
        Debug.Log($"Melee hit: {damage} damage to {target.name}");
    }

    private void ApplyKnockback(Collider target)
    {
        var rb = target.GetComponent<Rigidbody>();
        if (rb != null && _knockbackForce > 0)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0.3f; // Легкий подъем
            rb.AddForce(direction * _knockbackForce, ForceMode.Impulse);
        }
    }

    protected override void ResetAttackTimer()
    {
        float fireRate = _weaponSettings != null ?
            baseFireRate * GetTotalFireRateMultiplier() :
            1f * GetTotalFireRateMultiplier();

        _nextAttackTime = Time.time + (1f / fireRate);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.yellow;
        Vector3 leftDirection = Quaternion.Euler(0, -_attackAngle / 2, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, _attackAngle / 2, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, leftDirection * _attackRange);
        Gizmos.DrawRay(transform.position, rightDirection * _attackRange);
    }
}