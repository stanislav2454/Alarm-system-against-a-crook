using System;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Melee Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _damage = 25f;
    [SerializeField] private float _attackAngle = 90f;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private LayerMask _attackMask = ~0;

    //[Header("Melee Settings")]
    //[SerializeField] private MeleeWeaponSettings _meleeSettings;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem _attackEffect;
    [SerializeField] private AudioClip _attackSound;

    private AudioSource _audioSource;
    private float _finalDamage;

    protected override void Awake()
    {
        base.Awake();

        _audioSource = gameObject.AddComponent<AudioSource>();

        // Проверяем, есть ли настройки с интерфейсом IMeleeWeaponSettings
        if (_weaponSettings != null && _weaponSettings is IMeleeWeaponSettings meleeSettings)
        {
            _attackRange = meleeSettings.AttackRange;
            _damage = meleeSettings.BaseDamage;
            _attackAngle = meleeSettings.AttackAngle;
            _knockbackForce = meleeSettings.KnockbackForce;
            _attackMask = meleeSettings.AttackMask;
        }

        // Инициализируем урон
        _finalDamage = _weaponSettings != null ? baseDamage : _damage;
    }

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        NotifyDecoratorsOnAttack();

        // Обновляем урон с учетом множителей декораторов
        _finalDamage = (_weaponSettings != null ? baseDamage : _damage) * GetTotalDamageMultiplier();

        PlayAttackEffects();
        PerformMeleeAttack();
        //OnHit();
        ResetAttackTimer();
    }

    private void PlayAttackEffects()
    {
        if (_attackEffect != null)
            _attackEffect.Play();

        if (_attackSound != null && _audioSource != null)
            _audioSource.PlayOneShot(_attackSound);
    }

    private void PerformMeleeAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _attackRange, _attackMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject)
                continue;

            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= _attackAngle / 2)
            {
                ApplyDamage(hitCollider);
                ApplyKnockback(hitCollider);
            }
        }
    }

    private void ApplyDamage(Collider target)
    {
        var damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_finalDamage);
            Debug.Log($"Melee hit: {_finalDamage} damage to {target.name}");
        }
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
    //private void OnHit()
    //{
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, _attackRange, _attackMask);

    //    foreach (var hitCollider in hitColliders)
    //    {
    //        Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
    //        float angle = Vector3.Angle(transform.forward, directionToTarget);

    //        if (angle <= _attackAngle / 2)
    //        {
    //            var damageable = hitCollider.GetComponent<IDamageable>();
    //            damageable?.TakeDamage(_damage);
    //        }
    //    }
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    if (Application.isPlaying == false)
    //        return;

    //    // Сфера обнаружения
    //    Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
    //    Gizmos.DrawSphere(transform.position, _attackRange);

    //    // Сектор атаки
    //    Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
    //    Vector3 forward = transform.forward * _attackRange;
    //    Vector3 left = Quaternion.Euler(0, -_attackAngle / 2, 0) * forward;
    //    Vector3 right = Quaternion.Euler(0, _attackAngle / 2, 0) * forward;

    //    Gizmos.DrawRay(transform.position, left);
    //    Gizmos.DrawRay(transform.position, right);

    //    // Дуга сектора
    //    DrawAttackArc();
    //}

    //private void DrawAttackArc()
    //{
    //    Gizmos.color = Color.yellow;
    //    int segments = 20;
    //    float angleStep = _attackAngle / segments;
    //    Vector3 previousPoint = transform.position +
    //        Quaternion.Euler(0, -_attackAngle / 2, 0) * transform.forward * _attackRange;

    //    for (int i = 1; i <= segments; i++)
    //    {
    //        float angle = -_attackAngle / 2 + angleStep * i;
    //        Vector3 point = transform.position +
    //            Quaternion.Euler(0, angle, 0) * transform.forward * _attackRange;

    //        Gizmos.DrawLine(previousPoint, point);
    //        previousPoint = point;
    //    }
    //}
}