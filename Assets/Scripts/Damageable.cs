using UnityEngine;
using System;

public class Damageable : MonoBehaviour, IDamageable
{
    [SerializeField] private bool _destroyOnDeath = true;
    [SerializeField] private float _destroyDelay = 2f;

    public event Action OnDeath;
    public event Action<float> OnDamageTaken;
    public event Action<float> OnHealed;

    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
    [field: SerializeField] public bool IsPlayer { get; private set; } = false;
    public bool IsAlive { get; private set; } = true;
    public float HealthPercentage => MaxHealth > 0 ? Health / MaxHealth : 0;

    private void Start()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        Health = MaxHealth;
        IsAlive = true;
    }

    public void TakeDamage(float damage)
    {
        if (IsAlive == false || Health <= 0)
            return;

        if (damage <= 0)
            damage = 0;//ToDo: create Exeption

        Health = Mathf.Max(0, Health - damage);
        OnDamageTaken?.Invoke(Health);

        //Debug.Log($"{gameObject.name} took {damage} damage. Health: {Health}");

        if (Health <= 0)
            Die();
    }

    public void SetMaxHealth(float newMaxHealth, bool healToMax = false)
    {
        MaxHealth = Mathf.Max(1, newMaxHealth);

        if (healToMax)
        {
            Health = MaxHealth;
        }
        else
        {
            Health = Mathf.Min(Health, MaxHealth);
        }
    }

    public void Heal(float amount)
    {
        if (IsAlive == false || amount <= 0)
            return;

        float oldHealth = Health;
        Health = Mathf.Min(Health + amount, MaxHealth);
        float actualHeal = Health - oldHealth;

        if (actualHeal > 0)
            OnHealed?.Invoke(actualHeal);
    }

    public void Revive(float healthPercent = 1f)
    {
        if (IsAlive)
            return;

        IsAlive = true;
        Health = MaxHealth * Mathf.Clamp01(healthPercent);

        Debug.Log($"{gameObject.name} revived with {Health} health");
    }

    private void Die()
    {
        if (IsAlive == false)
            return;

        IsAlive = false;
        Debug.Log($"{gameObject.name} died!");

        OnDeath?.Invoke();

        if (IsPlayer == false)// Для врагов - просто уничтожаем
            Destroy(gameObject);
        // Для игрока GameManager сам обработает смерть
    }
}
