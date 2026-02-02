using UnityEngine;

public class ScopeDecorator : WeaponDecorator// - Прицел
{
    [Header("Scope Zoom")]
    [SerializeField] private float _zoomAmount = 30f; // Новый FOV при прицеливании
    [SerializeField] private float _zoomSpeed = 10f;   // Скорость зума

    [Header("Scope Effects")]
    [SerializeField] private GameObject _scopeOverlay; // UI оверлей прицела
    [SerializeField] private AudioClip _scopeSound;    // Звук прицеливания

    private Camera _playerCamera;
    private float _originalFOV;
    private bool _isScoped = false;

    private void Start()
    {
        _modifierName = "Scope";

        if (_decoratorSettings != null)
        {
            _damageMultiplier = _decoratorSettings.DamageMultiplier;
            _fireRateMultiplier = _decoratorSettings.FireRateMultiplier;

            if (_decoratorSettings.scopeZoomAmount > 0)
                _zoomAmount = _decoratorSettings.scopeZoomAmount;

            if (_decoratorSettings.scopeZoomSpeed > 0)
                _zoomSpeed = _decoratorSettings.scopeZoomSpeed;
        }
        else
        {
            _damageMultiplier = 1.2f;    // +20% урона при прицеливании
            _fireRateMultiplier = 1f;    // без изменения скорострельности
        }

        _playerCamera = Camera.main;
        if (_playerCamera != null)
            _originalFOV = _playerCamera.fieldOfView;

        if (_scopeOverlay != null)
            _scopeOverlay.SetActive(false);
    }

    private void Update()
    {
        if (_playerCamera == null)
            return;

        bool shouldScope = Input.GetMouseButton(1);//todo

        if (shouldScope != _isScoped)
        {
            _isScoped = shouldScope;
            OnScopeStateChanged(_isScoped);
        }

        float targetFOV = _isScoped ? _zoomAmount : _originalFOV;
        _playerCamera.fieldOfView = Mathf.Lerp(
                _playerCamera.fieldOfView, targetFOV, Time.deltaTime * _zoomSpeed);
    }

    public override float GetDamageMultiplier() => _isScoped ? _damageMultiplier : 1f;
    public override float GetFireRateMultiplier() => _isScoped ? _fireRateMultiplier : 1f;

    private void OnScopeStateChanged(bool scoped)
    {
        if (_scopeOverlay != null)// Показываем/скрываем оверлей прицела
            _scopeOverlay.SetActive(scoped);

        if (_scopeSound != null && GetComponent<AudioSource>() is AudioSource audio)
            audio.PlayOneShot(_scopeSound);

        Debug.Log(scoped ? "Scoped in" : "Scoped out");
    }

    private void OnDestroy()
    {
        if (_playerCamera != null)
            _playerCamera.fieldOfView = _originalFOV;

        if (_scopeOverlay != null)
            _scopeOverlay.SetActive(false);
    }
}