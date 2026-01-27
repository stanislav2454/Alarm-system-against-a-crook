using System;
using UnityEngine;

public abstract class WeaponDecorator : MonoBehaviour
{
    [SerializeField] protected string _modifierName = "Modifier";
    [SerializeField] protected float _damageMultiplier = 1f;
    [SerializeField] protected float _fireRateMultiplier = 1f;

    protected WeaponBase _attachedWeapon;
    protected DecoratorSettings _decoratorSettings;

    public string ModifierName => _modifierName;// ToDo to property

    public virtual void Initialize(WeaponSettings settings)
    {
        if (settings == null)
            throw new NullReferenceException();

        //// Используем as вместо is pattern matching для лучшей производительности
        //_decoratorSettings = settings as DecoratorSettings;

        //if (_decoratorSettings!=null)
        if (settings is DecoratorSettings decoratorSettings)
        {
            _decoratorSettings = decoratorSettings;
            _damageMultiplier = _decoratorSettings.DamageMultiplier;
            _fireRateMultiplier = _decoratorSettings.FireRateMultiplier;
            _modifierName = _decoratorSettings.ModifierName;
        }
        else
        {
            Debug.LogWarning($"WeaponDecorator: Expected DecoratorSettings but got {settings.GetType().Name}");
        }
    }

    public virtual void AttachToWeapon(WeaponBase weapon)
    {
        _attachedWeapon = weapon;
        Debug.Log($"{_modifierName} attached to {weapon.name}");
    }

    public virtual float GetDamageMultiplier() => // ?
        _damageMultiplier;

    public virtual float GetFireRateMultiplier() => // ?
        _fireRateMultiplier;

    public virtual void OnWeaponAttack()
    {
        // Переопределить в наследниках для логики во время атаки
        // Например: ScopeDecorator может добавить эффект прицеливания
    }

    public virtual void Remove()
    {
        Destroy(gameObject);
    }
    //protected virtual void ApplyModifiers()
    //{
    //    // Базовые модификаторы будут применены автоматически
    //    // Наследники могут переопределить для специфичной логики
    //}
}