using System.Collections;
using UnityEngine;

// Создайте временный тестовый скрипт
public class GameManagerTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(RunTests());
    }

    private IEnumerator RunTests()
    {
        Debug.Log("=== STARTING GAMEMANAGER TESTS ===");

        yield return new WaitForSeconds(1.5f);

        // Тест 1: Существует ли Instance?
        if (GameManager.Instance == null)
            Debug.LogError("FAIL: GameManager.Instance is null!");
        else
            Debug.Log("PASS: GameManager.Instance exists");

        // Тест 2: Есть ли игрок?
        if (GameManager.Instance.Player == null)
            Debug.LogError("FAIL: Player is not registered!");
        else
            Debug.Log($"PASS: Player registered: {GameManager.Instance.Player.name}");

        // Тест 3: Есть ли камера?
        if (GameManager.Instance.MainCamera == null)
            Debug.LogError("FAIL: Main Camera is null!");
        else
            Debug.Log($"PASS: Main Camera: {GameManager.Instance.MainCamera.name}");

        Debug.Log("=== TESTS COMPLETE ===");
    }
}
