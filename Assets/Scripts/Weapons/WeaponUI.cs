using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private Slider _reloadSlider;
    [SerializeField] private GameObject _reloadPanel;

    private WeaponInventory _weaponInventory;
    private RangeWeapon _currentRangeWeapon;

    private void Start()
    {
        var player = FindObjectOfType<Userinput>();// TODO: // Ищем инвентарь у игрока
        if (player != null)
        {
            _weaponInventory = player.GetComponent<WeaponInventory>();
        }

        if (_weaponInventory != null)
        {
            _weaponInventory.WeaponChanged += OnWeaponChanged;
            OnWeaponChanged(_weaponInventory.CurrentWeapon);
        }
        else
        {
            Debug.LogWarning("WeaponInventory not found!");
        }

        _reloadPanel.SetActive(false);
    }

    private void Update()
    {
        UpdateAmmoDisplay();
        UpdateReloadProgress();
    }

    private void OnDestroy()
    {
        if (_weaponInventory != null)
        {
            _weaponInventory.WeaponChanged -= OnWeaponChanged;
        }
    }

    private void OnWeaponChanged(WeaponBase newWeapon)
    {
        if (newWeapon != null)
        {
            _weaponNameText.text = newWeapon.Name;
            _currentRangeWeapon = newWeapon as RangeWeapon;
            UpdateAmmoDisplay();
        }
        else
        {
            _weaponNameText.text = "No Weapon";
            _currentRangeWeapon = null;
            UpdateAmmoDisplay();
        }
    }

    private void UpdateAmmoDisplay()
    {
        if (_currentRangeWeapon != null)
        {
            _ammoText.text = $"{_currentRangeWeapon.GetCurrentAmmo()} / {_currentRangeWeapon.GetMaxAmmo()}";
            _ammoText.gameObject.SetActive(true);
        }
        else
        {
            _ammoText.gameObject.SetActive(false);
        }
    }

    private void UpdateReloadProgress()
    {
        if (_currentRangeWeapon != null && _currentRangeWeapon.IsReloading)
        {
            _reloadPanel.SetActive(true);
            // Здесь можно добавить прогресс перезарядки
        }
        else
        {
            _reloadPanel.SetActive(false);
        }
    }
}