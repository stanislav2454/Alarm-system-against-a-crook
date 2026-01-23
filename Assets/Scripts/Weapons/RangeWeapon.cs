using UnityEngine;

public class RangeWeapon : WeaponBase
{
    [Header("Range Weapon Settings")]
    [SerializeField] protected float Range = 100f;
    [SerializeField] protected float Damage = 10f;
    [SerializeField] protected int MaxAmmo = 30;
    [SerializeField] protected float ReloadTime = 2f;
    [SerializeField] protected LayerMask AttackMask = ~0;

    [Header("Visual Effects")]
    [SerializeField] protected ParticleSystem MuzzleFlash;
    [SerializeField] protected GameObject ImpactEffect;//
    [SerializeField] protected AudioClip ShootSound;
    [SerializeField] protected AudioClip ReloadSound;
    [SerializeField] protected Transform FirePoint;

    protected int CurrentAmmo;

    protected AudioSource AudioSource;

    public bool IsReloading { get; private set; }

    protected virtual void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        if (AudioSource == null)
            AudioSource = gameObject.AddComponent<AudioSource>();

        AudioSource.playOnAwake = false;
        CurrentAmmo = MaxAmmo;
    }

    public int GetCurrentAmmo() =>
        CurrentAmmo;
    public int GetMaxAmmo() =>
        MaxAmmo;

    public override bool CanAttack()
    {
        return base.CanAttack() && IsReloading == false && CurrentAmmo > 0;
    }

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        Shoot();
        CurrentAmmo--;

        if (CurrentAmmo <= 0)
            StartReload();
    }

    protected virtual void Shoot()
    {
        if (MuzzleFlash != null)
            MuzzleFlash.Play();

        if (ShootSound != null)
            AudioSource.PlayOneShot(ShootSound);

        RaycastHit hit;
        if (Physics.Raycast(FirePoint.position, FirePoint.forward, out hit, Range, AttackMask))
            OnHit(hit);

        ResetAttackTimer();
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        var damageable = hit.collider.GetComponent<IDamageable>();
        damageable?.TakeDamage(Damage);

        if (ImpactEffect != null)
            Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public virtual void StartReload()
    {
        if (IsReloading == false && CurrentAmmo < MaxAmmo)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    protected virtual System.Collections.IEnumerator ReloadCoroutine()
    {
        IsReloading = true;

        if (ReloadSound != null)
            AudioSource.PlayOneShot(ReloadSound);

        yield return new WaitForSeconds(ReloadTime);

        CurrentAmmo = MaxAmmo;
        IsReloading = false;
    }
}