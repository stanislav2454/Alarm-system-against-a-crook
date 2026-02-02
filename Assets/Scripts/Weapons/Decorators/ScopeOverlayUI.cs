using UnityEngine;
using UnityEngine.UI;

public class ScopeOverlayUI : MonoBehaviour
{
    [Header("Scope Visuals")]
    [SerializeField] private Image _scopeImage;
    [SerializeField] private Color _scopeColor = Color.white;
    [SerializeField] private float _fadeSpeed = 8f;

    [Header("Scope Settings")]
    [SerializeField] private float _scopeScale = 1.5f;
    [SerializeField] private float _pulseSpeed = 2f;

    private float _targetAlpha = 0f;
    private float _currentAlpha = 0f;
    private RectTransform _rectTransform;

    private void Start()
    {
        if (_scopeImage != null)
        {
            _rectTransform = _scopeImage.GetComponent<RectTransform>();
            _scopeImage.color = new Color(_scopeColor.r, _scopeColor.g, _scopeColor.b, 0);
        }
    }

    private void Update()
    {
        if (_scopeImage == null)
            return;

        // Плавное изменение прозрачности
        _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, Time.deltaTime * _fadeSpeed);
        Color newColor = new Color(_scopeColor.r, _scopeColor.g, _scopeColor.b, _currentAlpha);

        _scopeImage.color = newColor;

        // Масштабирование
        if (_rectTransform != null)
        {
            float scale = 1f + (_currentAlpha * (_scopeScale - 1f));
            _rectTransform.localScale = Vector3.one * scale;
        }
    }
}