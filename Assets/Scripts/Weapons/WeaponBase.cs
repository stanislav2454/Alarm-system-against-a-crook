using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [Header("Basic Weapon Settings")]
    [SerializeField] protected WeaponSettings _weaponSettings;

    [Header("Decorators")]
    private List<WeaponDecorator> _decorators = new List<WeaponDecorator>();//

    protected float _nextAttackTime;
    protected float baseDamage;
    protected float baseFireRate;
    protected float baseAttackInterval;
    protected string weaponName;

    public string Name => _weaponSettings?.weaponName ?? gameObject.name;
    public float BaseDamage => baseDamage * GetTotalDamageMultiplier();
    public virtual float BaseFireRate => baseFireRate * GetTotalFireRateMultiplier();
    public WeaponSettings Settings => _weaponSettings;// ?

    protected virtual void Awake()
    {
        if (_weaponSettings != null)
            InitializeFromSettings();
        else
            InitializeDefaults();
    }

    public virtual bool CanAttack() => Time.time >= _nextAttackTime;// todo to property ?

    public abstract void Attack();

    public void AddDecorator(WeaponDecorator decoratorPrefab)
    {
        if (decoratorPrefab == null)
            return;

        WeaponDecorator newDecorator = Instantiate(decoratorPrefab, transform);

        if (_weaponSettings is DecoratorSettings decoratorSettings)
            newDecorator.Initialize(_weaponSettings);

        newDecorator.AttachToWeapon(this);
        _decorators.Add(newDecorator);
        Debug.Log($"Added {newDecorator.ModifierName}");
    }

    // Метод для установки настроек в рантайме
    public virtual void SetWeaponSettings(WeaponSettings settings)
    {
        _weaponSettings = settings;
        if (settings != null)
            InitializeFromSettings();
    }

    protected void NotifyDecoratorsOnAttack()
    {
        foreach (var decorator in _decorators)
            decorator.OnWeaponAttack();
    }

    protected float GetTotalDamageMultiplier()
    {
        if (_decorators.Count == 0)
            return 1f;

        float total = 1f;
        foreach (var decorator in _decorators)
            total *= decorator.GetDamageMultiplier();

        return total;
    }

    protected float GetTotalFireRateMultiplier()
    {
        if (_decorators.Count == 0)
            return 1f;
        //float total = AttackRate;
        float total = 1f;// Начинаем с 1, а не с AttackRate
        foreach (var decorator in _decorators)
            total *= decorator.GetFireRateMultiplier();

        return total;
    }

    protected virtual void ResetAttackTimer()
    {
        float finalFireRateMultiplier = GetTotalFireRateMultiplier();
        _nextAttackTime = Time.time + (baseAttackInterval / finalFireRateMultiplier);
    }

    private void InitializeDefaults()
    {
        baseDamage = 5f;
        baseFireRate = 1f;
        baseAttackInterval = 1f;
        weaponName = "Unnamed Weapon";
        Debug.LogWarning($"WeaponSettings not assigned to {gameObject.name}! Using default values.");
    }

    private void InitializeFromSettings()
    {
        baseDamage = _weaponSettings.GetBaseDamage();
        baseFireRate = _weaponSettings.baseFireRate * _weaponSettings.globalFireRateMultiplier;
        baseAttackInterval = _weaponSettings.GetBaseAttackInterval();
        weaponName = _weaponSettings.weaponName;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 5);
    }
}