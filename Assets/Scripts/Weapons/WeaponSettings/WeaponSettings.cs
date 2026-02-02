using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponSettings", menuName = "Weapons/Weapon Settings")]
public class WeaponSettings : ScriptableObject, IWeaponSettings
{
    [Header("Basic Settings")]
    public string weaponName = "Weapon";
    public float baseDamage = 10f;
    public float baseFireRate = 1f; // выстрелов в секунду

    [Header("Balance Adjustments")]
    [Range(0.1f, 10f)]
    public float globalDamageMultiplier = 1f;
    [Range(0.1f, 10f)]
    public float globalFireRateMultiplier = 1f;

    // Реализация интерфейса IWeaponSettings
    public string WeaponName => weaponName;
    public float BaseDamage => baseDamage * globalDamageMultiplier;
    public float BaseFireRate => baseFireRate * globalFireRateMultiplier;

    // Для совместимости со старым кодом
    public float GetBaseAttackInterval() => 1f / BaseFireRate;
    public float GetBaseDamage() => BaseDamage;
}

// Отдельные ScriptableObject для разных типов оружия
[CreateAssetMenu(fileName = "RangeWeaponSettings", menuName = "Weapons/Range Weapon Settings")]
public class RangeWeaponSettings : WeaponSettings, IRangeWeaponSettings
{
    [Header("Range Weapon Settings")]
    public float range = 100f;
    public int maxAmmo = 30;
    public float reloadTime = 2f;
    public LayerMask attackMask = ~0;
    public float baseSpread = 0.5f;

    // Реализация IRangeWeaponSettings
    public float Range => range;// ?
    public int MaxAmmo => maxAmmo;// ?
    public float ReloadTime => reloadTime;// ?
    public LayerMask AttackMask => attackMask;// ?
    public float BaseSpread => baseSpread;// ?

    // Для удобства доступа в коде
    public new float GetBaseDamage() => base.GetBaseDamage();// ?
    public new float GetBaseAttackInterval() => base.GetBaseAttackInterval();// ?
}


[CreateAssetMenu(fileName = "MeleeWeaponSettings", menuName = "Weapons/Melee Weapon Settings")]
public class MeleeWeaponSettings : WeaponSettings, IMeleeWeaponSettings
{
    [Header("Melee Weapon Settings")]
    public float attackRange = 2f;
    public float attackAngle = 90f;
    public float knockbackForce = 5f;
    public LayerMask attackMask = ~0;

    // Реализация IMeleeWeaponSettings
    public float AttackRange => attackRange;// ?
    public float AttackAngle => attackAngle;// ?
    public float KnockbackForce => knockbackForce;// ?
    public LayerMask AttackMask => attackMask;// ?
}

[CreateAssetMenu(fileName = "DecoratorSettings", menuName = "Weapons/Decorator Settings")]
public class DecoratorSettings : WeaponSettings, IDecoratorSettings
{
    [Header("Decorator Settings")]
    public string modifierName = "Decorator";
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;

    [Header("Scope Specific")]
    public float scopeZoomAmount = 30f;
    public float scopeZoomSpeed = 10f;
    public float scopeDamageMultiplier = 1.2f;
    public float scopeFireRateMultiplier = 0.9f;

    [Header("Extended Mag Specific")]
    public float extendedMagMultiplier = 1.5f;
    public float extendedMagReloadMultiplier = 1.2f;

    [Header("Silencer Specific")]
    public float silencerNoiseReduction = 0.3f;
    public float silencerDamageMultiplier = 0.8f;
    public float silencerFireRateMultiplier = 1f;

    // Реализация IDecoratorSettings
    public string ModifierName => modifierName;
    public float DamageMultiplier => damageMultiplier;
    public float FireRateMultiplier => fireRateMultiplier;
    public float ScopeZoomAmount => scopeZoomAmount;
    public float ScopeZoomSpeed => scopeZoomSpeed;
    public float ExtendedMagMultiplier => extendedMagMultiplier;
    public float ExtendedMagReloadMultiplier => extendedMagReloadMultiplier;
    public float SilencerNoiseReduction => silencerNoiseReduction;
}
