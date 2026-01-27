using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleEnemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _attackCooldown = 1f;

    [Header("References")]
    [SerializeField] private LayerMask _playerLayer = 1 << 6; // Layer 6 = Player

    private Damageable _damageable;
    private Transform _player;
    private CharacterController _controller;
    private float _lastAttackTime;

    // Статическое событие для отслеживания убийств
    public static event Action EnemyKilled;

    private void OnEnable()
    {
        if (_damageable != null)
        {
            _damageable.OnDeath += HandleDeath;
            _damageable.OnDamageTaken += HandleDamageTaken;
        }
    }

    private void OnDisable()
    {
        if (_damageable != null)
        {
            _damageable.OnDeath -= HandleDeath;
            _damageable.OnDamageTaken -= HandleDamageTaken;
        }
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _damageable = GetComponent<Damageable>();

        if (_damageable == null)
        {
            Debug.LogError("SimpleEnemy requires Damageable component!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (_player == null || _damageable.IsAlive == false || enabled == false)
            return;

        UpdateEnemy();
    }

    public void Initialize()
    {
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            _player = GameManager.Instance.Player.Transform;
        }
        else
        {
            Debug.LogWarning("Player not found via GameManager. Enemy disabled.");
            enabled = false;
        }
    }

    protected virtual void UpdateEnemy()
    {
        Vector3 direction = GetDirectionToPlayer();
        RotateTowardsPlayer(direction);

        float distance = direction.magnitude;

        if (distance > _attackRange)
            MoveToPlayer(direction, distance);
        else
            TryAttackPlayer();
    }

    private Vector3 GetDirectionToPlayer()
    {
        return _player.position - transform.position;
    }

    private void RotateTowardsPlayer(Vector3 direction)
    {
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);// ToDo: magic num
        }
    }

    private void MoveToPlayer(Vector3 direction, float distance)
    {
        Vector3 moveDirection = direction.normalized;
        _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);
    }

    private void TryAttackPlayer()
    {
        if (Time.time - _lastAttackTime >= _attackCooldown)
        {
            AttackPlayer();
            _lastAttackTime = Time.time;
        }
    }

    private void HandleDamageTaken(float currentHealth)
    {
        //Debug.Log($"Enemy took damage! Health: {currentHealth}");

        // Потом можно добавить:
        // - Звук получения урона
        // - Изменение поведения
        // - Визуальную обратную связь
    }

    private void HandleDeath()
    {
        Debug.Log($"{name} died!");

        // Вызываем событие убийства
        EnemyKilled?.Invoke();

        // Отключаем логику врага
        enabled = false;

        // Потом можно добавить:
        // - Анимацию смерти
        // - Эффекты
        // - Выпадение лута
        // - Возврат в пул

        Destroy(gameObject, 2f); //ToDo. Задержка для анимации
    }

    private void AttackPlayer()
    {
        Debug.Log($"{name} attacks player!");

        if (Vector3.Distance(transform.position, _player.position) <= _attackRange)//ToDo
        {
            if (GameManager.Instance?.Player?.Health != null)
                GameManager.Instance.Player.Health.TakeDamage(_attackDamage);
        }
    }

    private void OnDestroy()
    {
        _damageable.OnDeath -= HandleDeath;
        _damageable.OnDamageTaken -= HandleDamageTaken;
    }
}