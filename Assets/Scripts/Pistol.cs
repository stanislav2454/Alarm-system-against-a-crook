using UnityEngine;

public class Pistol : RangeWeapon
{
    [Header("Pistol Settings")]
    [SerializeField] private float recoilForce = 0.1f;

    protected override void Shoot()
    {
        base.Shoot();

        // Отдача для пистолета
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        // Простая отдача ( улучшить в будущем )
        if (Camera.main != null)
        {
            Camera.main.transform.localEulerAngles += new Vector3(-recoilForce, 0, 0);
        }
    }
}