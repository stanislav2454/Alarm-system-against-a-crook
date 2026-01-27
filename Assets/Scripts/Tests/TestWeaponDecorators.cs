using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestWeaponDecorators : MonoBehaviour
{
    [Header("Test References")]
    [SerializeField] private WeaponBase _testWeapon;
    [SerializeField] private Camera _playerCamera;

    [Header("Decorator Prefabs")]
    [SerializeField] private ScopeDecorator _scopePrefab;
    [SerializeField] private ExtendedMagDecorator _magPrefab;
    [SerializeField] private SilencerDecorator _silencerPrefab;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Image _scopeOverlay;
    [SerializeField] private Slider _fovSlider;

    [Header("Test Controls")]
    [SerializeField] private KeyCode _toggleScopeKey = KeyCode.T;
    [SerializeField] private KeyCode _addScopeKey = KeyCode.Alpha0;
    [SerializeField] private KeyCode _addMagKey = KeyCode.Alpha9;
    [SerializeField] private KeyCode _addSilencerKey = KeyCode.Alpha8;
    [SerializeField] private KeyCode _removeAllKey = KeyCode.U;
    [SerializeField] private KeyCode _fireTestKey = KeyCode.F;

    private float _originalFOV = 60f;
    private bool _hasScope = false;
    private ScopeDecorator _currentScope;

    private void Start()
    {
        if (_playerCamera != null)
        {
            _originalFOV = _playerCamera.fieldOfView;
        }

        if (_fovSlider != null)
        {
            _fovSlider.minValue = 20f;
            _fovSlider.maxValue = _originalFOV;
            _fovSlider.value = _originalFOV;
        }

        UpdateUI();
    }

    private void Update()
    {
        HandleTestInput();
        UpdateFOVSlider();
        UpdateUI();
    }

    private void HandleTestInput()
    {
        // Переключение прицеливания (правая кнопка мыши или T)
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(_toggleScopeKey))
            ToggleScope();

        // Добавление декораторов
        if (Input.GetKeyDown(_addScopeKey))
            AddScopeDecorator();

        if (Input.GetKeyDown(_addMagKey))
            AddDecorator(_magPrefab);

        if (Input.GetKeyDown(_addSilencerKey))
            AddDecorator(_silencerPrefab);

        // Удаление всех
        if (Input.GetKeyDown(_removeAllKey))
            RemoveAllDecorators();

        // Тест выстрела
        if (Input.GetKeyDown(_fireTestKey) && _testWeapon != null)
            TestFire();
    }

    private void ToggleScope()
    {
        if (_hasScope == false || _currentScope == null)
            return;

        // ScopeDecorator сам управляет зумом через Update()
        // Мы просто показываем визуальную обратную связь
        if (_scopeOverlay != null)
            _scopeOverlay.enabled = !_scopeOverlay.enabled;

        Debug.Log($"Scope toggled. Right-click to aim.");
    }

    private void AddScopeDecorator()
    {
        if (_testWeapon == null || _scopePrefab == null)
            return;

        if (_hasScope == false)
        {
            _testWeapon.AddDecorator(_scopePrefab);
            _hasScope = true;

            // Получаем ссылку на ScopeDecorator
            _currentScope = _testWeapon.GetComponentInChildren<ScopeDecorator>();

            if (_currentScope != null)
                Debug.Log($"Scope added. Use Right Mouse Button to aim.");

        }
        else
        {
            Debug.Log("Scope already added!");
        }
    }

    private void AddDecorator(WeaponDecorator decoratorPrefab)
    {
        if (_testWeapon == null || decoratorPrefab == null)
            return;

        _testWeapon.AddDecorator(decoratorPrefab);
    }

    private void RemoveAllDecorators()
    {
        if (_testWeapon == null)
            return;

        // Восстанавливаем FOV
        if (_playerCamera != null)
            _playerCamera.fieldOfView = _originalFOV;

        // Скрываем оверлей
        if (_scopeOverlay != null)
            _scopeOverlay.enabled = false;

        _hasScope = false;
        _currentScope = null;

        Debug.Log("All decorators removed. FOV reset.");
    }

    private void TestFire()
    {
        if (_testWeapon.CanAttack())
        {

            Debug.Log("BANG! Test fire.");// Имитируем выстрел

            if (_hasScope && Input.GetMouseButton(1))// Если есть Scope - показываем эффект прицеливания            
                Debug.Log("Scoped shot: +20% damage");
        }
        else
        {
            Debug.Log("Can't attack yet (fire rate cooldown)");
        }
    }

    private void UpdateFOVSlider()
    {
        if (_playerCamera != null && _fovSlider != null)
            _fovSlider.value = _playerCamera.fieldOfView;
    }

    private void UpdateUI()
    {
        if (_infoText == null)
            return;

        string info = "=== WEAPON DECORATOR TEST ===\n\n";

        if (_testWeapon != null)
        {
            info += $"Weapon: {_testWeapon.Name}\n";
            info += $"Can Attack: {_testWeapon.CanAttack()}\n";
        }

        info += $"\nDecorators:\n";
        info += $"Scope: {(_hasScope ? "INSTALLED" : "NOT INSTALLED")}\n";

        if (_hasScope && _currentScope != null)
        {
            bool isAiming = Input.GetMouseButton(1);
            info += $"  Aiming: {(isAiming ? "YES (RMB)" : "NO")}\n";
            info += $"  Damage Multi: x{_currentScope.GetDamageMultiplier():F1}\n";
            info += $"  Fire Rate Multi: x{_currentScope.GetFireRateMultiplier():F1}\n";
        }

        if (_playerCamera != null)
        {
            info += $"\nCamera FOV: {_playerCamera.fieldOfView:F1}\n";
            info += $"Original FOV: {_originalFOV}\n";
        }

        info += $"\n=== CONTROLS ===\n";
        info += $"RMB or T: Toggle Scope\n";
        info += $"{_addScopeKey}: Add Scope\n";
        info += $"{_addMagKey}: Add Extended Mag\n";
        info += $"{_addSilencerKey}: Add Silencer\n";
        info += $"{_removeAllKey}: Remove All\n";
        info += $"F: Test Fire\n";

        _infoText.text = info;
    }

    // Для UI Slider
    public void OnFOVSliderChanged(float value)
    {
        if (_playerCamera != null)
        {
            _playerCamera.fieldOfView = value;
            _originalFOV = value; // Обновляем оригинальное значение
        }
    }

    private void OnDrawGizmos()
    {
        if (_testWeapon != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_testWeapon.transform.position, 0.5f);
        }
    }
}