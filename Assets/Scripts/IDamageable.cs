public interface IDamageable
{
    public void TakeDamage(float damage);
    // Можно расширить при необходимости:
    // float Health { get; }
    // bool IsAlive { get; }
}