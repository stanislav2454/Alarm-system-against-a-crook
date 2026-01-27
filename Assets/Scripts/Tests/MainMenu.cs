// MainMenu.cs (для временного меню)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private TMP_Text _versionText;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (_startButton != null)
            _startButton.onClick.AddListener(StartGame);

        if (_quitButton != null)
            _quitButton.onClick.AddListener(QuitGame);

        if (_versionText != null)
            _versionText.text = $"Version {Application.version}";
    }

    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneLoader.LoadGameScene();
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
}