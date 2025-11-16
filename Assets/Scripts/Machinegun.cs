using UnityEngine;

public class Machinegun : RangeWeapon
{
    [Header("Auto Rifle Settings")]
    [SerializeField] private float _spreadAngle = 2f;

    protected override void Shoot()
    {
        // Добавляем разброс для автомата
        Vector3 spread = FirePoint.forward;
        spread += new Vector3(
            Random.Range(-_spreadAngle, _spreadAngle) * 0.01f,
            Random.Range(-_spreadAngle, _spreadAngle) * 0.01f,
            0);

        RaycastHit hit;
        if (Physics.Raycast(FirePoint.position, spread, out hit, Range, AttackMask))
            OnHit(hit);

        if (MuzzleFlash != null)
            MuzzleFlash.Play();

        if (ShootSound != null)
            AudioSource.PlayOneShot(ShootSound);

        CurrentAmmo--;
        ResetAttackTimer();
    }
}
