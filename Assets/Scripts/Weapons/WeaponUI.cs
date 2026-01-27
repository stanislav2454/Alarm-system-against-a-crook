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
    [SerializeField] private TextMeshProUGUI _reloadTimeText; // Дополнительный текст для времени


    private WeaponInventory _weaponInventory;
    private RangeWeapon _currentRangeWeapon;
    private float _reloadStartTime;
    private float _reloadDuration;

    private void Start()
    {
        var player = FindObjectOfType<Userinput>();// TODO: // Ищем инвентарь у игрока
        if (player != null)
            _weaponInventory = player.GetComponent<WeaponInventory>();

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
        _reloadSlider.gameObject.SetActive(false);

        if (_reloadTimeText != null)
            _reloadTimeText.gameObject.SetActive(false);
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
            _currentRangeWeapon.ReloadStarted -= OnReloadStarted;
            _currentRangeWeapon.ReloadFinished -= OnReloadFinished;
        }
    }

    private void OnWeaponChanged(WeaponBase newWeapon)
    {
        if (newWeapon != null)
        {
            _weaponNameText.text = newWeapon.Name;
            _currentRangeWeapon = newWeapon as RangeWeapon;

            // Подписываемся на события перезарядки нового оружия
            if (_currentRangeWeapon != null)
            {
                _currentRangeWeapon.ReloadStarted += OnReloadStarted;
                _currentRangeWeapon.ReloadFinished += OnReloadFinished;
            }

            UpdateAmmoDisplay();
        }
        else
        {
            _weaponNameText.text = "No Weapon";

            // Отписываемся от событий старого оружия
            if (_currentRangeWeapon != null)
            {
                _currentRangeWeapon.ReloadStarted -= OnReloadStarted;
                _currentRangeWeapon.ReloadFinished -= OnReloadFinished;
            }

            _currentRangeWeapon = null;
            UpdateAmmoDisplay();
        }
        //if (newWeapon != null)
        //{
        //    _weaponNameText.text = newWeapon.Name;
        //    _currentRangeWeapon = newWeapon as RangeWeapon;
        //    UpdateAmmoDisplay();
        //}
        //else
        //{
        //    _weaponNameText.text = "No Weapon";
        //    _currentRangeWeapon = null;
        //    UpdateAmmoDisplay();
        //}
    }

    private void OnReloadStarted(float duration)
    {
        _reloadStartTime = Time.time;
        _reloadDuration = duration;
        _reloadPanel.SetActive(true);
        _reloadSlider.gameObject.SetActive(true);

        if (_reloadTimeText != null)
            _reloadTimeText.gameObject.SetActive(true);

        _reloadSlider.minValue = 0;
        _reloadSlider.maxValue = 1;
        _reloadSlider.value = 0;
    }

    private void OnReloadFinished()
    {
        _reloadPanel.SetActive(false);
        _reloadSlider.gameObject.SetActive(false);

        if (_reloadTimeText != null)
            _reloadTimeText.gameObject.SetActive(false);
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
            // Расчет прогресса перезарядки
            float elapsedTime = Time.time - _reloadStartTime;
            float progress = Mathf.Clamp01(elapsedTime / _reloadDuration);

            // Обновление слайдера
            _reloadSlider.value = progress;

            // Обновление текста времени (опционально)
            if (_reloadTimeText != null)
            {
                float remainingTime = Mathf.Max(0, _reloadDuration - elapsedTime);
                _reloadTimeText.text = $"{remainingTime:F1}s";
            }
        }
        //if (_currentRangeWeapon != null && _currentRangeWeapon.IsReloading)
        //{
        //    _reloadPanel.SetActive(true);
        //    // Здесь можно добавить прогресс перезарядки
        //}
        //else
        //{
        //    _reloadPanel.SetActive(false);
        //}
    }
}