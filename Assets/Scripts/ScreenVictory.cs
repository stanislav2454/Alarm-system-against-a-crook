// VictoryScreen.cs - ПОЛНАЯ ВЕРСИЯ
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScreenVictory : MonoBehaviour
//public class VictoryScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private TextMeshProUGUI _wavesCompletedText;
    [SerializeField] private TextMeshProUGUI _enemiesKilledText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;

    [Header("Animation")]
    [SerializeField] private float _showDelay = 1f;
    [SerializeField] private AnimationCurve _fadeCurve;
    [SerializeField] private CanvasGroup _canvasGroup;

    // Статистика
    private int _totalEnemiesKilled = 0;
    private int _wavesCompleted = 0;
    private float _gameStartTime;

    private WaveManager _waveManager;
    private PlayerController _player;

    private void Start()
    {
        Initialize();

        SimpleEnemy.EnemyKilled += OnEnemyKilled;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))// Тестовая кнопка
            ShowVictoryScreen();
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (_waveManager != null)
        {
            _waveManager.WaveCompleted -= OnWaveCompleted;
            _waveManager.AllWavesComplete -= OnAllWavesComplete;
        }
    }

    private void Initialize()
    {
        // Находим менеджеры
        _waveManager = FindObjectOfType<WaveManager>();// ToDo
        _player = FindObjectOfType<PlayerController>();// ToDo

        _gameStartTime = Time.time;

        SetupButtons();

        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);

        SubscribeToEvents();
        UpdateStatsUI();
    }

    private void SetupButtons()
    {
        if (_restartButton != null)
            _restartButton.onClick.AddListener(RestartGame);

        if (_mainMenuButton != null)
            _mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (_quitButton != null)
            _quitButton.onClick.AddListener(QuitGame);
    }

    private void SubscribeToEvents()
    {
        if (_waveManager != null)
        {
            _waveManager.WaveCompleted += OnWaveCompleted;
            _waveManager.AllWavesComplete += OnAllWavesComplete;
        }

        // Подписка на убийства врагов (нужно добавить в SimpleEnemy)
        // SimpleEnemy.OnDeath += () => _totalEnemiesKilled++;
    }

    private void OnWaveCompleted(int waveIndex)
    {
        _wavesCompleted = waveIndex + 1;
        UpdateStatsUI();
    }

    private void OnAllWavesComplete()
    {
        // Ждем немного перед показом экрана
        Invoke(nameof(ShowVictoryScreen), _showDelay);
    }

    private void ShowVictoryScreen()
    {
        Debug.Log("Showing victory screen!");
        UpdateFinalStats();

        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(true);

            if (_canvasGroup != null)
                StartCoroutine(FadeInCoroutine());
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (_player != null)
            _player.enabled = false;

        if (_waveManager != null)
            _waveManager.enabled = false;

        // Можно остановить врагов
        PauseAllEnemies();
    }

    private void UpdateFinalStats()
    {
        // Время игры
        float gameTime = Time.time - _gameStartTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
        string timeString = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";

        // Обновляем UI
        if (_wavesCompletedText != null)
            _wavesCompletedText.text = $"Waves Completed: {_wavesCompleted}/3";

        if (_enemiesKilledText != null)
            _enemiesKilledText.text = $"Enemies Killed: {_totalEnemiesKilled}";

        if (_timeText != null)
            _timeText.text = $"Time: {timeString}";
    }

    private void UpdateStatsUI()
    {
        // Можно обновлять статистику в реальном времени
        // (например, в углу экрана во время игры)
    }

    private System.Collections.IEnumerator FadeInCoroutine()
    {
        _canvasGroup.alpha = 0f;
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _canvasGroup.alpha = _fadeCurve.Evaluate(t);
            yield return null;
        }

        _canvasGroup.alpha = 1f;
    }

    private void PauseAllEnemies()
    {
        var enemies = FindObjectsOfType<SimpleEnemy>();// todo
        foreach (var enemy in enemies)
            enemy.enabled = false;
    }

    // ===== КНОПКИ =====
    public void RestartGame()
    {
        Debug.Log("Restarting game...");

        // Вместо SceneLoader.LoadGameScene() используем:

        // Вариант 1: Просто перезагружаем текущую сцену
        SceneLoader.ReloadCurrentScene();

        // Вариант 2: Если хотите через главное меню
        // SceneLoader.LoadMainMenu();

        // Вариант 3: С эффектом затухания
        /*
        if (_canvasGroup != null)
        {
            StartCoroutine(FadeOutAndRestart());
        }
        else
        {
            SceneLoader.ReloadCurrentScene();
        }
        */
    }

    private System.Collections.IEnumerator FadeOutAndRestart()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _canvasGroup.alpha = 1f - t;
            yield return null;
        }

        SceneLoader.LoadGameScene();
    }

    public void GoToMainMenu()
    {
        Debug.Log("Going to main menu...");

        SaveGameStats();// Сохраняем статистику если нужно

        SceneLoader.LoadMainMenu();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SaveGameStats()
    {
        // Сохраняем статистику в PlayerPrefs
        PlayerPrefs.SetInt("LastGame_Waves", _wavesCompleted);
        PlayerPrefs.SetInt("LastGame_Kills", _totalEnemiesKilled);
        PlayerPrefs.SetFloat("LastGame_Time", Time.time - _gameStartTime);
        PlayerPrefs.Save();
    }

    private void OnEnemyKilled()
    {
        _totalEnemiesKilled++;
        UpdateStatsUI();
    }

    public void AddEnemyKill()
    {
        _totalEnemiesKilled++;
        UpdateStatsUI();
    }
}