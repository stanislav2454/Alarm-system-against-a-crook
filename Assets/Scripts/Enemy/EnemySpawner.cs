using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public float weight = 1f; // Вероятность спавна
    }

    [Header("Enemy Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _maxEnemies = 5;
    [SerializeField] private float _spawnInterval = 2f;

    [Header("Spawn Points")]
    [SerializeField] private SpawnPoint[] _spawnPoints;

    [Header("Arena")]
    [SerializeField] private float _arenaRadius = 20f;

    private int _currentEnemies = 0;
    private float _nextSpawnTime;

    private void Start()
    {
        _nextSpawnTime = Time.time + _spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime && _currentEnemies < _maxEnemies)
        {
            SpawnEnemy();
            _nextSpawnTime = Time.time + _spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefab == null)
            return;

        Vector3 spawnPosition = GetSpawnPosition();

        GameObject enemyObj = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        _currentEnemies++;

        SimpleEnemy enemy = enemyObj.GetComponent<SimpleEnemy>();
        if (enemy != null) // Враг автоматически получит ссылку на игрока через GameManager
            enemy.Initialize();

        var health = enemyObj.GetComponent<Damageable>();
        if (health != null)
            health.OnDeath += () => OnEnemyDeath();

        Debug.Log($"Enemy spawned at {spawnPosition}. Total: {_currentEnemies}");
    }

    private Vector3 GetSpawnPosition()
    {
        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            float totalWeight = 0;
            foreach (var point in _spawnPoints)
                totalWeight += point.weight;

            float randomValue = Random.Range(0, totalWeight);
            float currentWeight = 0;

            foreach (var point in _spawnPoints)
            {
                currentWeight += point.weight;

                if (randomValue <= currentWeight)
                    return point.point.position;
            }

            return _spawnPoints[0].point.position;
        }

        float angle = Random.Range(0, 360) * Mathf.Deg2Rad; // ToDo
        float distance = _arenaRadius + 5f; // ToDo

        return new Vector3(Mathf.Cos(angle) * distance,
                            0,
                            Mathf.Sin(angle) * distance);
    }

    private void OnEnemyDeath()
    {
        _currentEnemies--;
        Debug.Log($"Enemy died. Remaining: {_currentEnemies}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, _arenaRadius);

        if (_spawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (var point in _spawnPoints)
            {
                if (point.point != null)
                {
                    Gizmos.DrawSphere(point.point.position, 0.5f);
                    Gizmos.DrawWireSphere(point.point.position, 1f);
                }
            }
        }
    }
}