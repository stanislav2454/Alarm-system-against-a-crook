using System;
using UnityEngine;

public class ExtendedMagDecorator : WeaponDecorator// - Увеличенный магазин
{
    [SerializeField] private int extraAmmo = 10;

    private void Start()
    {
        _modifierName = "Extended Mag";
        _damageMultiplier = 1f;      // Не влияет на урон
        _fireRateMultiplier = 1f;    // Не влияет на скорострельность
    }

    public override void AttachToWeapon(WeaponBase weapon)
    {
        base.AttachToWeapon(weapon);

        if (weapon is RangeWeapon rangeWeapon)
        {
            // Здесь нужно добавить патроны
            Debug.Log($"Added {extraAmmo} extra ammo");
        }
    }

    internal float GetReloadTimeMultiplier()
    {
        throw new NotImplementedException();
    }
}