using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenPause : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Userinput _userInput;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;

    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private AnimationCurve _fadeCurve;
    [SerializeField] private CanvasGroup _canvasGroup;

    private PlayerController _player;
    private bool _isPaused;
    private Coroutine _currentFadeCoroutine;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();// ToDo Лучше использовать ссылку

        if (_panel != null)
            _panel.SetActive(false);

        if (_canvasGroup != null)
            _canvasGroup.alpha = 0f;

        SetupButtons();
    }

    void Update()
    {
        //Используйте - Свойство с публичным getter.
        //Свойства с лямбда - выражениями будут вычисляться на каждый вызов, что идеально для клавиши паузы.

        //Если планируете расширять систему ввода(например, добавлять поддержку геймпада, перепривязку клавиш),
        //лучше использовать вариант с событиями
        // - это более гибкая архитектура.
        if (_userInput != null && _userInput.IsPausePressed)// todo
        //if (Input.GetKeyDown(KeyCode.Escape))// todo
        {
            if (_isPaused == false)
                ShowPauseScreen();
            else
                ClosePauseScreen();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Сбрасываем паузу
        Debug.Log("Restarting game...");
        SceneLoader.ReloadCurrentScene();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Сбрасываем паузу
        Debug.Log("Going to main menu...");
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

    private void SetupButtons()
    {
        if (_restartButton != null)
            _restartButton.onClick.AddListener(RestartGame);

        if (_mainMenuButton != null)
            _mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (_quitButton != null)
            _quitButton.onClick.AddListener(QuitGame);
    }

    private void ShowPauseScreen()
    {
        if (_isPaused)
            return;

        _isPaused = true;
        Time.timeScale = 0f;

        if (_panel != null)
        {
            _panel.SetActive(true);

            // Анимация появления
            if (_canvasGroup != null)
                if (_currentFadeCoroutine != null)
                    StopCoroutine(_currentFadeCoroutine);

            _currentFadeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f));
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (_player != null)
            _player.enabled = false;

        SetEnemiesPauseState(true);
    }

    private void ClosePauseScreen()
    {
        if (_isPaused == false)
            return;

        _isPaused = false;
        Time.timeScale = 1f;// Возобновляем игровое время

        // Анимация исчезновения
        if (_canvasGroup != null)
        {
            if (_currentFadeCoroutine != null)
                StopCoroutine(_currentFadeCoroutine);

            _currentFadeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, () =>
                    {
                        if (_panel != null)
                            _panel.SetActive(false);
                    }));
        }
        else
        {
            if (_panel != null)
                _panel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_player != null)
            _player.enabled = true;

        SetEnemiesPauseState(false);
    }

    private IEnumerator FadeCoroutine(float fromAlpha, float toAlpha, System.Action onComplete = null)
    {
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Используем unscaledDeltaTime для работы во время паузы
            float t = elapsed / _fadeDuration;
            _canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, _fadeCurve.Evaluate(t));
            yield return null;
        }

        _canvasGroup.alpha = toAlpha;
        _currentFadeCoroutine = null;

        onComplete?.Invoke();
    }

    private void SetEnemiesPauseState(bool paused)
    {
        var enemies = FindObjectsOfType<SimpleEnemy>();// Переделать
        foreach (var enemy in enemies)
            enemy.enabled = !paused;
    }

    private void OnDestroy()
    {
        // Всегда сбрасываем таймскейл при уничтожении
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }
}
