using UnityEngine;

public class TestDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100f;

    public void TakeDamage(float damage)
    {
        _health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {_health}");

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}