using System.Collections.Generic;

public interface IWeaponDecorator : IWeapon
{// НЕНУЖЕН - УДАЛИТЬ
    public IWeapon DecoratedWeapon { get; }

    public void Initialize(IWeapon weaponToDecorate);

    public float GetTotalDamage();

    public float GetTotalFireRate();

    public string GetFullDescription();

    public List<string> GetActiveModifiers();

    public void ApplyDamageModifier(float multiplier);

    public void ApplyFireRateModifier(float multiplier);

    public void RemoveDecorator();
}