using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<PlayerController> OnPlayerRegistered;
    public event Action PlayerDeath;
    public event Action GameRestart;

    private PlayerController _player;
    private Camera _mainCamera;

    public PlayerController Player => _player;
    public Camera MainCamera => _mainCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _mainCamera = Camera.main;
    }

    public void RegisterPlayer(PlayerController player)
    {
        _player = player;
        OnPlayerRegistered?.Invoke(player);
        Debug.Log($"Player registered: {player.name}");
    }

    public void UnregisterPlayer(PlayerController player)
    {
        if (_player == player)
            _player = null;
    }

    public void PlayerDied()
    {
        PlayerDeath?.Invoke();
        RestartGame();
    }

    private void RestartGame()
    {
        Debug.Log("Game restarting...");
        GameRestart?.Invoke();

        // Пока просто перезагрузка сцены
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}