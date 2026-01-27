using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool _showDebugInfo = true;

    private void Update()
    {
        // Быстрая проверка в консоли
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log($"<color=red>GameManager Status:</color>");
            Debug.Log($"- Instance: {Instance != null}");
            Debug.Log($"- <color=#FFA500>Player:</color> <color=#FFFF00>{_player?.name ?? "NULL"}</color>");
            //Debug.Log($"- <color=#FFA500>Player:</color> {_player?.name ?? "NULL"}");
            // #FFA500 = стандартный orange
            //Debug.Log($"- <color=rgba(255,128,0,1)>Player:</color> {_player?.name ?? "NULL"}");// does not work
            Debug.Log($"- Camera: {_mainCamera?.name ?? "NULL"}");
        }
    }

    private void OnGUI()
    {
        if (_showDebugInfo == false)
            return;

        const int Width = 800;
        const int Height = 400;

        // Стиль для заголовка
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 28,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.red },
            alignment = TextAnchor.MiddleCenter
        };

        // Стиль для обычного текста
        GUIStyle normalStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            normal = { textColor = Color.yellow },
            fontStyle = FontStyle.Normal
        };

        //// Создаем стиль с нужным размером шрифта
        //GUIStyle largeLabelStyle = new GUIStyle(GUI.skin.label);
        //largeLabelStyle.fontSize = 20; // Размер шрифта
        //largeLabelStyle.fontStyle = FontStyle.Bold; // Дополнительно можно задать стиль

        GUILayout.BeginArea(new Rect(800, 10, Width, Height));
        //GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== GAME MANAGER DEBUG ===", headerStyle);
        GUILayout.Label($"Instance: {Instance != null}", normalStyle);
        GUILayout.Label($"Player Registered: {_player != null}", normalStyle);

        if (_player != null)
        {
            GUILayout.Label($"Player Name: {_player.name}");
            GUILayout.Label($"- Player HP: {_player.Health.Health}", normalStyle);
            GUILayout.Label($"Player Position: {_player.Position}");
        }

        GUILayout.Label($"Main Camera: {_mainCamera != null}");
        GUILayout.EndArea();
    }
    //
    public static GameManager Instance { get; private set; }

    public event Action<PlayerController> PlayerRegistered;
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
        if (player == null)
            throw new ArgumentNullException(player.name);

        _player = player;

        //Invoke(nameof(TestReg), 0.5f);

        PlayerRegistered?.Invoke(player);
        Debug.Log($"Player registered: {player.name}");
    }
    //void TestReg(PlayerController player)
    //{
    //    PlayerRegistered?.Invoke(player);
    //}

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