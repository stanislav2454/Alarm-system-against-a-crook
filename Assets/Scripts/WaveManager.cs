// WaveManager.cs
using UnityEngine;
using System;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private WaveData _waveData;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _bossPrefab; // Отдельный префаб босса
    [SerializeField] private Transform[] _spawnPoints;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _waveText;
    //[SerializeField] private UnityEngine.UI.Text _waveText;
    [SerializeField] private TextMeshProUGUI _enemiesLeftText;
    //[SerializeField] private UnityEngine.UI.Text _enemiesLeftText;

    // Состояние
    private int _currentWaveIndex = 0;
    private int _enemiesAlive = 0;
    private int _enemiesToSpawn = 0;
    private float _nextSpawnTime = 0;
    private bool _isWaveActive = false;

    // События
    public event Action<int> WaveStarted; // waveIndex
    public event Action<int> WaveCompleted; // waveIndex
    public event Action AllWavesComplete;

    public int CurrentWave => _currentWaveIndex + 1;
    public int TotalWaves => _waveData?.waves.Length ?? 0;
    public bool IsWaveActive => _isWaveActive;

    private void Start()
    {
        if (_waveData == null)
        {
            Debug.LogError("WaveData not assigned!");
            return;
        }

        StartNextWave();
    }

    private void Update()
    {
        if (_isWaveActive == false)
            return;

        HandleWaveSpawning();
        UpdateUI();
    }

    public void StartNextWave()
    {
        if (_currentWaveIndex >= TotalWaves)
        {
            Debug.Log("All waves completed!");
            AllWavesComplete?.Invoke();
            return;
        }

        var currentWave = _waveData.waves[_currentWaveIndex];
        _enemiesToSpawn = currentWave.enemyCount + (currentWave.hasBoss ? 1 : 0);
        _enemiesAlive = _enemiesToSpawn;
        _isWaveActive = true;
        _nextSpawnTime = Time.time;

        Debug.Log($"Starting {currentWave.waveName}");
        WaveStarted?.Invoke(_currentWaveIndex);

        UpdateUI();
    }

    private void HandleWaveSpawning()
    {
        if (_enemiesToSpawn <= 0) 
            return;

        var currentWave = _waveData.waves[_currentWaveIndex];

        if (Time.time >= _nextSpawnTime)
        {
            SpawnEnemy(currentWave);
            _enemiesToSpawn--;
            _nextSpawnTime = Time.time + currentWave.spawnInterval;
        }
    }

    private void SpawnEnemy(WaveData.EnemyWave wave)
    {
        if (_enemyPrefab == null) 
            return;

        // Определяем префаб (обычный враг или босс)
        bool spawnBoss = wave.hasBoss && _enemiesToSpawn == 1; // Босс последним
        GameObject prefabToSpawn = spawnBoss && _bossPrefab != null ? _bossPrefab : _enemyPrefab;

        // Выбираем точку спавна
        Transform spawnPoint = GetSpawnPoint();
        if (spawnPoint == null) return;

        // Создаем врага
        GameObject enemyObj = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // Настраиваем врага
        var enemy = enemyObj.GetComponent<SimpleEnemy>();
        var damageable = enemyObj.GetComponent<Damageable>();

        if (enemy != null)
        {
            enemy.Initialize();
        }

        if (damageable != null)
        {
            // Усиливаем босса
            if (spawnBoss)
            {
                damageable.SetMaxHealth(damageable.MaxHealth * wave.bossHealthMultiplier, true);
                enemyObj.name = "Boss " + enemyObj.name;
            }

            // Подписываемся на смерть
            damageable.OnDeath += OnEnemyDeath;
        }

        Debug.Log($"Spawned {(spawnBoss ? "BOSS" : "enemy")} at wave {CurrentWave}");
    }

    private Transform GetSpawnPoint()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return transform;
        }

        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
    }

    private void OnEnemyDeath()
    {
        _enemiesAlive--;

        if (_enemiesAlive <= 0)
        {
            CompleteCurrentWave();
        }

        UpdateUI();
    }

    private void CompleteCurrentWave()
    {
        _isWaveActive = false;

        Debug.Log($"Wave {CurrentWave} completed!");
        WaveCompleted?.Invoke(_currentWaveIndex);

        _currentWaveIndex++;

        // Автоматически запускаем следующую волну через 3 секунды
        if (_currentWaveIndex < TotalWaves)
        {
            Invoke(nameof(StartNextWave), 3f);
        }
        else
        {
            AllWavesComplete?.Invoke();
            ShowVictoryScreen();
        }
    }

    private void UpdateUI()
    {
        if (_waveText != null)
            _waveText.text = $"Wave: {CurrentWave}/{TotalWaves}";

        if (_enemiesLeftText != null)
            _enemiesLeftText.text = $"Enemies: {_enemiesAlive}";
    }

    private void ShowVictoryScreen()
    {
        Debug.Log("=== VICTORY! All waves completed! ===");

        // Простое окно победы в консоли
        // Позже заменим на UI
    }

    private void OnDrawGizmos()
    {
        if (_spawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (var point in _spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.5f);
                    Gizmos.DrawWireSphere(point.position, 1f);
                }
            }
        }
    }
}