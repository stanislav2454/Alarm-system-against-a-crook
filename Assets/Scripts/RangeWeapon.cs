using UnityEngine;

public class RangeWeapon : WeaponBase
{
    [Header("Range Weapon Settings")]
    [SerializeField] protected float range = 100f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected int maxAmmo = 30;
    [SerializeField] protected float reloadTime = 2f;
    [SerializeField] protected LayerMask attackMask = ~0;

    [Header("Visual Effects")]
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject impactEffect;
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected Transform firePoint;

    protected int currentAmmo;
    protected bool isReloading;

    protected AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        currentAmmo = maxAmmo;
    }

    public override bool CanAttack()
    {
        return base.CanAttack() && !isReloading && currentAmmo > 0;
    }

    public override void Attack()
    {
        if (CanAttack() == false)
            return;

        Shoot();
        currentAmmo--;

        if (currentAmmo <= 0)
            StartReload();
    }

    protected virtual void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range, attackMask))
            OnHit(hit);

        ResetAttackTimer();
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        var damageable = hit.collider.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage);

        if (impactEffect != null)
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public virtual void StartReload()
    {
        if (isReloading == false && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    protected virtual System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        if (reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}