using UnityEngine;

public class Pistol : RangeWeapon
{
    [Header("Pistol Settings")]
    [SerializeField] private float _recoilForce = 0.1f;

    protected override void Shoot()
    {
        base.Shoot();

        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        // Простая отдача ( улучшить в будущем )
        if (Camera.main != null)
        {
            Camera.main.transform.localEulerAngles += new Vector3(-_recoilForce, 0, 0);
        }
    }
}