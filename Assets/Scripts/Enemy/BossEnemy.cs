// BossEnemy.cs
using UnityEngine;

public class BossEnemy : SimpleEnemy
{
    [Header("Boss Settings")]
    [SerializeField] private float _areaAttackRange = 5f;
    [SerializeField] private float _areaAttackDamage = 10f;
    [SerializeField] private float _areaAttackCooldown = 3f;
    [SerializeField] private ParticleSystem _areaAttackEffect;

    private float _lastAreaAttackTime;

    protected override void UpdateEnemy()
    {
        base.UpdateEnemy();

        // Босс иногда делает area attack
        if (Time.time - _lastAreaAttackTime >= _areaAttackCooldown)
        {
            TryAreaAttack();
            _lastAreaAttackTime = Time.time;
        }
    }

    private void TryAreaAttack()
    {
        Debug.Log("BOSS AREA ATTACK!");

        if (_areaAttackEffect != null)
            _areaAttackEffect.Play();

        // Найти всех игроков в радиусе
        Collider[] hits = Physics.OverlapSphere(transform.position, _areaAttackRange);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_areaAttackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _areaAttackRange);
    }
}