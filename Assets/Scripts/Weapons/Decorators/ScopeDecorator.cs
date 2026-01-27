using UnityEngine;

public class ScopeDecorator : WeaponDecorator// - Прицел
{
    [Header("Scope Zoom")]
    [SerializeField] // Эти значения теперь берутся из WeaponSettings
    private float _zoomAmount = 30f; // Новый FOV при прицеливании
    [SerializeField]
    private float _zoomSpeed = 10f;   // Скорость зума

    [Header("Scope Effects")]
    [SerializeField] private GameObject scopeOverlay; // UI оверлей прицела
    [SerializeField] private AudioClip scopeSound;    // Звук прицеливания

    private Camera _playerCamera;
    private float _originalFOV;
    private bool _isScoped = false;

    private void Start()
    {
        _modifierName = "Scope";

        // Если есть DecoratorSettings, используем значения из них
        if (_decoratorSettings != null)
        {
            _damageMultiplier = _decoratorSettings.DamageMultiplier;
            _fireRateMultiplier = _decoratorSettings.FireRateMultiplier;

            // Используем значения из DecoratorSettings, если они установлены
            if (_decoratorSettings.scopeZoomAmount > 0)
                _zoomAmount = _decoratorSettings.scopeZoomAmount;

            if (_decoratorSettings.scopeZoomSpeed > 0)
                _zoomSpeed = _decoratorSettings.scopeZoomSpeed;
        }
        else
        {
            // Значения по умолчанию
            _damageMultiplier = 1.2f;    // +20% урона при прицеливании
            _fireRateMultiplier = 1f;    // без изменения скорострельности
        }
        //if (_attachedWeapon == null)
        //{
        //    Debug.LogError("ScopeDecorator not attached to weapon!");
        //    return;
        //}
        ////_modifierName = "Scope";
        ////_damageMultiplier = 1.2f;    // +20% урона при прицеливании
        ////_fireRateMultiplier = 1f;  // -20% скорострельности (для баланса)
        //////_fireRateMultiplier = 0.9f;  // -20% скорострельности (для баланса)

        _playerCamera = Camera.main;
        if (_playerCamera != null)
            _originalFOV = _playerCamera.fieldOfView;

        if (scopeOverlay != null)
            scopeOverlay.SetActive(false);
    }

    private void Update()
    {
        if (_playerCamera == null)
            return;

        // Прицеливание по правой кнопке мыши
        bool shouldScope = Input.GetMouseButton(1);//todo

        if (shouldScope != _isScoped)
        {
            _isScoped = shouldScope;
            OnScopeStateChanged(_isScoped);
        }

        // Плавный зум
        float targetFOV = _isScoped ? _zoomAmount : _originalFOV;
        _playerCamera.fieldOfView = Mathf.Lerp(
                _playerCamera.fieldOfView, targetFOV, Time.deltaTime * _zoomSpeed);
    }

    //public override void Initialize(WeaponSettings settings)
    //{
    //    base.Initialize(settings);
    //    _modifierName = "Scope";
    //    _damageMultiplier = settings.scopeDamageMultiplier;
    //    _fireRateMultiplier = settings.scopeFireRateMultiplier;
    //    _zoomAmount = settings.scopeZoomAmount;
    //    _zoomSpeed = settings.scopeZoomSpeed;
    //}

    private void OnScopeStateChanged(bool scoped)
    {
        if (scopeOverlay != null)// Показываем/скрываем оверлей прицела
            scopeOverlay.SetActive(scoped);

        if (scopeSound != null && GetComponent<AudioSource>() is AudioSource audio)
            audio.PlayOneShot(scopeSound);

        Debug.Log(scoped ? "Scoped in" : "Scoped out");
    }

    public override float GetDamageMultiplier()
    {
        // Дополнительный урон ТОЛЬКО при прицеливании
        return _isScoped ? _damageMultiplier : 1f;
    }

    public override float GetFireRateMultiplier()
    {
        // Замедление ТОЛЬКО при прицеливании
        return _isScoped ? _fireRateMultiplier : 1f;
        //return _isScoped ? _fireRateMultiplier : 0.5f;
    }

    private void OnDestroy()
    {
        if (_playerCamera != null)
            _playerCamera.fieldOfView = _originalFOV;

        if (scopeOverlay != null)
            scopeOverlay.SetActive(false);
    }
}