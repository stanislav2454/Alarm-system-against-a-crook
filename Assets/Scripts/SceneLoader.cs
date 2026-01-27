// SceneLoader.cs - ИСПРАВЛЕННЫЙ
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Индексы сцен в Build Settings
    public const int MAIN_MENU_INDEX = 0;
    public const int GAME_SCENE_INDEX = 1;

    public static void LoadGameScene()
    {
        Debug.Log($"Loading game scene (index: {GAME_SCENE_INDEX})");

        // Проверяем, есть ли сцена в Build Settings
        if (GAME_SCENE_INDEX < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(GAME_SCENE_INDEX);
        }
        else
        {
            Debug.LogError($"Scene index {GAME_SCENE_INDEX} not in build settings!");
            Debug.LogWarning("Total scenes in build: " + SceneManager.sceneCountInBuildSettings);

            // Альтернатива: загружаем по имени (если знаем имя)
            LoadSceneByName("GameScene");
        }
    }

    public static void LoadMainMenu()
    {
        Debug.Log($"Loading main menu (index: {MAIN_MENU_INDEX})");

        if (MAIN_MENU_INDEX < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(MAIN_MENU_INDEX);
        }
        else
        {
            Debug.LogError($"Main menu index {MAIN_MENU_INDEX} not in build settings!");
            LoadSceneByName("MainMenu");
        }
    }

    private static void LoadSceneByName(string sceneName)
    {
        // Пытаемся загрузить по имени
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load scene '{sceneName}': {e.Message}");
            Debug.LogWarning("Reloading current scene instead...");
            ReloadCurrentScene();
        }
    }

    public static void ReloadCurrentScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }

    // Новый метод для безопасной загрузки
    public static void LoadScene(int buildIndex)
    {
        if (buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            Debug.LogError($"Invalid scene index: {buildIndex}");
            ReloadCurrentScene();
        }
    }

    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // Метод для проверки, добавлена ли сцена
    public static bool IsSceneInBuild(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (System.IO.Path.GetFileNameWithoutExtension(scenePath) == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}