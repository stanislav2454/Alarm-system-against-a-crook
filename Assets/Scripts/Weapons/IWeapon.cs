public interface IWeapon
{
    public string Name { get; }
    public bool CanAttack();
    public void Attack();
}