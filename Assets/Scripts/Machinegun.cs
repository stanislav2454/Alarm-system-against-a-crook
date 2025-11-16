using UnityEngine;

public class Machinegun : RangeWeapon// переименовать в machinegun
{
    [Header("Auto Rifle Settings")]
    [SerializeField] private float spreadAngle = 2f;

    protected override void Shoot()
    {
        // Добавляем разброс для автомата
        Vector3 spread = firePoint.forward;
        spread += new Vector3(
            Random.Range(-spreadAngle, spreadAngle) * 0.01f,
            Random.Range(-spreadAngle, spreadAngle) * 0.01f,
            0);

        // Raycast с разбросом
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, spread, out hit, range, attackMask))
            OnHit(hit);

        // Визуальные эффекты (из базового класса)
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);

        currentAmmo--;
        ResetAttackTimer();
    }
}
