using System.Collections;
using UnityEngine;

public class Pistol : RangeWeapon
{
    [Header("Pistol Settings")]
    [SerializeField] private float _recoilForce = 0.1f;
    [SerializeField] private float _recoilRecoverySpeed = 5f;

    //private WeaponSettings _settings;
    private float _currentRecoil = 0f;
    private Coroutine _recoilRecoveryCoroutine;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    _settings = _weaponSettings;
    //}

    protected override void Shoot(float damage)
    {
        base.Shoot(damage);

        ApplyRecoil();// Очень сомнительная ф-ция - скорее всего нужно убрать
    }

    private void ApplyRecoil()
    {
        // Простая отдача ( улучшить в будущем )
        if (Camera.main != null)
        {
            _currentRecoil += _recoilForce;// Добавляем отдачу

            // Поворачиваем камеру
            Camera.main.transform.localEulerAngles = new Vector3(
                    -_currentRecoil, Camera.main.transform.localEulerAngles.y, 0);

            if (_recoilRecoveryCoroutine != null)// Запускаем/перезапускаем восстановление
                StopCoroutine(_recoilRecoveryCoroutine);

            _recoilRecoveryCoroutine = StartCoroutine(RecoilRecovery());
        }
    }

    private IEnumerator RecoilRecovery()
    {
        while (_currentRecoil > 0.01f)
        {
            _currentRecoil = Mathf.Lerp(_currentRecoil, 0f, Time.deltaTime * _recoilRecoverySpeed);

            if (Camera.main != null)
                Camera.main.transform.localEulerAngles = new Vector3(
                        -_currentRecoil, Camera.main.transform.localEulerAngles.y, 0);

            yield return null;
        }

        _currentRecoil = 0f;
        if (Camera.main != null)
            Camera.main.transform.localEulerAngles = new Vector3(
                    0, Camera.main.transform.localEulerAngles.y, 0);
    }

    private void OnDestroy()
    {
        if (_recoilRecoveryCoroutine != null)
            StopCoroutine(_recoilRecoveryCoroutine);

        if (Camera.main != null)
            Camera.main.transform.localEulerAngles = new Vector3(
                    0, Camera.main.transform.localEulerAngles.y, 0);
    }
}