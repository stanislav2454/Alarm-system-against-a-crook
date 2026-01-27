using UnityEngine;

public interface IWeaponSettings
{
    public string WeaponName { get; }
    public float BaseDamage { get; }
    public float BaseFireRate { get; }
}

public interface IMeleeWeaponSettings : IWeaponSettings
{
    public float AttackRange { get; }
    public float AttackAngle { get; }
    public float KnockbackForce { get; }
    public LayerMask AttackMask { get; }
}

public interface IRangeWeaponSettings : IWeaponSettings
{
    public float Range { get; }
    public int MaxAmmo { get; }
    public float ReloadTime { get; }
    public LayerMask AttackMask { get; }
    public float BaseSpread { get; }
}

public interface IDecoratorSettings
{
    string ModifierName { get; }
    float DamageMultiplier { get; }
    float FireRateMultiplier { get; }
    float ScopeZoomAmount { get; }
    float ScopeZoomSpeed { get; }
    float ExtendedMagMultiplier { get; }
    float ExtendedMagReloadMultiplier { get; }
    float SilencerNoiseReduction { get; }
}