using UnityEngine;

public class SilencerDecorator : WeaponDecorator// - глушитель
{
    [SerializeField] private AudioClip _silencedSound;

    private AudioSource _weaponAudioSource;

    private void Start()
    {
        _modifierName = "Silencer";
        _damageMultiplier = 0.8f;
        _fireRateMultiplier = 1f;
    }

    public override void AttachToWeapon(WeaponBase weapon)
    {
        base.AttachToWeapon(weapon);

        if (weapon.TryGetComponent(out _weaponAudioSource) == false)
            Debug.LogError($"Не получилось установить AudioSource для {GetType().Name}");
    }

    public override void OnWeaponAttack()
    {
        if (_silencedSound != null)// Воспроизводим тихий звук вместо обычного
            _weaponAudioSource.PlayOneShot(_silencedSound);
    }
}