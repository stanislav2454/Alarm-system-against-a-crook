using System;
using System.Collections;
using UnityEngine;

public class RangeWeapon : WeaponBase
{
    [Header("Range Weapon Settings")]
    [SerializeField] protected float Range = 100f;
    // [SerializeField] protected float Damage = 10f;
    [SerializeField] protected int MaxAmmo = 30;
    [SerializeField] protected float ReloadTime = 2f;
    [SerializeField] protected LayerMask AttackMask = ~0;
    [SerializeField] protected float BaseSpread = 0.5f;
    [SerializeField] protected float AttackRate = 1f;// Добавляем поле для базовой скорости атаки (для совместимости)

    [Header("Visual Effects")]
    [SerializeField] protected ParticleSystem MuzzleFlash;
    [SerializeField] protected GameObject ImpactEffect;//
    [SerializeField] protected AudioClip ShootSound;
    [SerializeField] protected AudioClip ReloadSound;
    [SerializeField] protected Transform FirePoint;

    //protected bool IsReloading = false;
    protected int CurrentAmmo;
    protected AudioSource AudioSource;
    protected float CurrentSpread = 0f;
    protected float MaxSpread = 2f;
    protected float SpreadPerShot = 0.1f;
    protected float SpreadRecoveryRate = 5f;

    public event Action<float> ReloadStarted;
    public event Action ReloadFinished;

    public bool IsReloading { get; private set; } = false;
    public int GetCurrentAmmo() => CurrentAmmo;//todo
    public int GetMaxAmmo() => MaxAmmo;//todo
    public override bool CanAttack() => base.CanAttack() && IsReloading == false && CurrentAmmo > 0;

    protected virtual void Start()
    {
        //base.Start(); // Вызываем Awake из WeaponBase
        AudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        AudioSource.playOnAwake = false;

        if (_weaponSettings is RangeWeaponSettings rangeSettings)
            InitializeFromRangeSettings(rangeSettings);
        else
            CurrentAmmo = MaxAmmo;
    }

    protected virtual void Update()
    {
        if (CurrentSpread > 0)
            CurrentSpread = Mathf.Max(CurrentSpread - Time.deltaTime * SpreadRecoveryRate, 0f);
    }

    private void InitializeFromRangeSettings(RangeWeaponSettings rangeSettings)
    {
        Range = rangeSettings.Range;
        MaxAmmo = rangeSettings.MaxAmmo;
        ReloadTime = rangeSettings.ReloadTime;
        AttackMask = rangeSettings.AttackMask;
        BaseSpread = rangeSettings.BaseSpread;
        CurrentAmmo = MaxAmmo;
    }

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        NotifyDecoratorsOnAttack();// Уведомляем декораторы

        float finalDamage = GetBaseDamage();
        // Используем урон из WeaponSettings или локальный
        //float finalDamage = _weaponSettings != null ?
        //        baseDamage * GetTotalDamageMultiplier() :
        //        Damage * GetTotalDamageMultiplier();

        Shoot(finalDamage);
        CurrentAmmo--;

        // Увеличение разброса
        CurrentSpread = Mathf.Min(CurrentSpread + SpreadPerShot, MaxSpread);

        if (CurrentAmmo <= 0)
            StartReload();

        ResetAttackTimer();
    }

    protected virtual void Shoot(float damage)
    {
        if (MuzzleFlash != null)
            MuzzleFlash.Play();

        if (ShootSound != null)
            AudioSource.PlayOneShot(ShootSound);

        Vector3 spreadDirection = GetSpreadDirection();

        RaycastHit hit;
        if (Physics.Raycast(FirePoint.position, spreadDirection, out hit, Range, AttackMask))
            OnHit(hit, damage);
    }

    protected virtual Vector3 GetSpreadDirection()
    {
        Vector3 direction = FirePoint.forward;

        if (CurrentSpread > 0)
        {
            float spreadAmount = Mathf.Lerp(BaseSpread, MaxSpread, CurrentSpread / MaxSpread);
            direction += new Vector3(
                    UnityEngine.Random.Range(-spreadAmount, spreadAmount) * 0.01f,
                    UnityEngine.Random.Range(-spreadAmount, spreadAmount) * 0.01f,
                    0);
            direction.Normalize();
        }

        return direction;
    }

    protected virtual void OnHit(RaycastHit hit, float damage)
    {
        hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage);

        if (ImpactEffect != null)
            Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public virtual void StartReload()
    {
        if (IsReloading == false && CurrentAmmo < MaxAmmo)
            StartCoroutine(ReloadCoroutine());
    }

    protected virtual IEnumerator ReloadCoroutine()
    {
        IsReloading = true;

        float reloadMultiplier = 1f;
        float totalReloadTime = ReloadTime * reloadMultiplier;

        // Вызываем событие начала перезарядки с длительностью
        ReloadStarted?.Invoke(totalReloadTime);

        if (ReloadSound != null)
            AudioSource.PlayOneShot(ReloadSound);

        foreach (var decorator in GetComponentsInChildren<WeaponDecorator>())
            if (decorator is ExtendedMagDecorator extendedMag)
                reloadMultiplier = extendedMag.GetReloadTimeMultiplier();

        yield return new WaitForSeconds(totalReloadTime);

        CurrentAmmo = MaxAmmo;
        IsReloading = false;
        CurrentSpread = 0f; // Сброс разброса при перезарядке
        ReloadFinished?.Invoke();// Перезарядка завершена
    }

    //protected override void ResetAttackTimer()
    //{
    //    // Учитываем множитель скорострельности
    //    float fireRate = AttackRate * GetTotalFireRateMultiplier();
    //    _nextAttackTime = Time.time + (1f / fireRate);
    //}
    protected override void ResetAttackTimer()
    {
        float baseRate = _weaponSettings != null ? baseFireRate : AttackRate;
        float finalFireRate = baseRate * GetTotalFireRateMultiplier();
        _nextAttackTime = Time.time + (1f / finalFireRate);
    }
}