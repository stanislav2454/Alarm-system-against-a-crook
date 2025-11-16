using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Melee Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _damage = 25f;
    [SerializeField] private LayerMask _attackMask = ~0;
    [SerializeField] private float _attackAngle = 90f;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem _attackEffect;
    [SerializeField] private AudioClip _attackSound;

    private AudioSource _audioSource;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        PlayAttackEffects();
        CheckHit();
        ResetAttackTimer();
    }

    private void PlayAttackEffects()
    {
        if (_attackEffect != null)
            _attackEffect.Play();

        if (_attackSound != null && _audioSource != null)
            _audioSource.PlayOneShot(_attackSound);
    }

    private void CheckHit()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _attackRange, _attackMask);

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= _attackAngle / 2)
            {
                var damageable = hitCollider.GetComponent<IDamageable>();
                damageable?.TakeDamage(_damage);
            }
        }
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