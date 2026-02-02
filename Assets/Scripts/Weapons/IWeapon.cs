public interface IWeapon
{
    public string Name { get; }
    public bool CanAttack();
    public void Attack();
}

// Интерфейс для оружия с поддержкой модификаций
public interface IModifiableWeapon
{
    public int GetOriginalMagazineSize();
    public float GetOriginalReloadTime();
    public void SetReloadTimeMultiplier(float multiplier);
    public void ResetReloadTime();
}

// Интерфейс для оружия с патронами
public interface IWeaponWithAmmo
{
    public int CurrentAmmo { get; }
    public int GetMagazineCapacity();
    void ModifyMagazineCapacity(int v);//?
}