using UnityEngine;

public class Machinegun : RangeWeapon
{
    [Header("Auto Rifle Settings")]
    [SerializeField] private float _spreadAngle = 2f;

    //private WeaponSettings _settings;

    //protected override void Awake()
    //{
    //    base.Awake();

    //    // Получаем настройки из базового класса
    //    _settings = _weaponSettings;
    //}

    protected override void Shoot(float damage)
    {
        Vector3 spread = GetSpreadDirection();// Добавляем разброс для автомата
        spread += new Vector3(
            Random.Range(-_spreadAngle, _spreadAngle) * 0.01f,
            Random.Range(-_spreadAngle, _spreadAngle) * 0.01f,
            0);

        RaycastHit hit;
        if (Physics.Raycast(FirePoint.position, spread, out hit, Range, AttackMask))
            OnHit(hit, damage);

        if (MuzzleFlash != null)
            MuzzleFlash.Play();

        if (ShootSound != null)
            AudioSource.PlayOneShot(ShootSound);

        // Таймер сброса теперь вызывается в базовом классе RangeWeapon
        //CurrentAmmo--;
        //ResetAttackTimer();
    }
}
