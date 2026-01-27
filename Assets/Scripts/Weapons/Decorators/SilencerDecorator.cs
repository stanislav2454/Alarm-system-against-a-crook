using UnityEngine;

public class SilencerDecorator : WeaponDecorator// - глушитель
{
    [SerializeField] private AudioClip _silencedSound;

    private AudioSource _weaponAudio;

    private void Start()
    {
        _modifierName = "Silencer";
        _damageMultiplier = 0.8f;    // -20% урона
        _fireRateMultiplier = 1f;    // Не влияет на скорострельность
    }

    public override void AttachToWeapon(WeaponBase weapon)
    {
        base.AttachToWeapon(weapon);

        _weaponAudio = weapon.GetComponent<AudioSource>();
    }

    public override void OnWeaponAttack()
    {
        // Воспроизводим тихий звук вместо обычного
        if (_silencedSound != null)
            _weaponAudio.PlayOneShot(_silencedSound);
    }
}