using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRaycaster : MonoBehaviour
{
    [Header("Настройки луча")]
    [SerializeField] private float _rayLength = 40f;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private bool _showDebugRay = true;

    [Header("Цвета")]
    [SerializeField] private Color _rayColor = Color.magenta;
    [SerializeField] private Color _hitColor = Color.yellow;
    [SerializeField] private Color _missColor = Color.red;
    //[SerializeField] private LayerMask hitLayers = ~0; // По умолчанию все слои

    // Для хранения данных о последнем попадании
    private bool _lastHitValid = false;
    private RaycastHit _lastHit;
    private bool _hitDamageable = false;

    public bool IsHittingDamageable => _hitDamageable;

    private void OnValidate()
    {
        if (_rayOrigin == null)
            Debug.LogError("Не установлена точка начала луча!");
    }

    void Update()
    {
        PerformRaycast();
    }

    private void PerformRaycast()
    {
        Ray ray = new Ray(_rayOrigin.position, _rayOrigin.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayLength))
        //if (Physics.Raycast(ray, out hit, rayLength, hitLayers))
        {
            _lastHitValid = true;
            _lastHit = hit;

            _hitDamageable = hit.transform.TryGetComponent(out Damageable target);

            if (_hitDamageable)
                Debug.Log($"Ray hit to: {target.gameObject.name}\n Hp: {target.Health}");

        }
        else
        {
            _lastHitValid = false;
            _hitDamageable = false;
        }

        ShowRay();
    }

    private void ShowRay()
    {
        if (_showDebugRay)
        {
            float distance = _lastHitValid ? _lastHit.distance : _rayLength;
            Debug.DrawRay(_rayOrigin.position, _rayOrigin.forward * distance, _rayColor);
        }
    }

    private void OnDrawGizmos()
    {
        if (_showDebugRay == false || _rayOrigin == null)
            return;

        Gizmos.color = _rayColor;
        float distance = _lastHitValid ? _lastHit.distance : _rayLength;
        Gizmos.DrawRay(_rayOrigin.position, _rayOrigin.forward * distance);

        if (_lastHitValid)
        {
            Gizmos.color = _hitDamageable ? _hitColor : _missColor;
            Gizmos.DrawSphere(_lastHit.point, _radius);
            //Gizmos.DrawWireSphere(_lastHit.point, _radius);
        }
    }

    public RaycastHit? GetLastHitInfo()
    {
        if (_lastHitValid)
            return _lastHit;

        return null;
    }

    //public RaycastHit GetLastHitInfo()
    //{
    //    if (_lastHitValid)
    //        return _lastHit;
    //    // Что возвращать, если попадания не было?
    //    // Нельзя вернуть null для структуры!
    //}
}
