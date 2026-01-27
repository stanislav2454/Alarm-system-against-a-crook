using System;
using UnityEngine;

// Создайте тестовый подписчик
public class EventTester : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.PlayerRegistered += OnPlayerRegistered;
        GameManager.Instance.PlayerDeath += OnPlayerDeath;
        GameManager.Instance.GameRestart += OnGameRestart;
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerRegistered -= OnPlayerRegistered;
        GameManager.Instance.PlayerDeath -= OnPlayerDeath;
        GameManager.Instance.GameRestart -= OnGameRestart;
    }

    private void OnPlayerRegistered(PlayerController player)
    {
        if (player == null)
            throw new ArgumentNullException(player.name);
        // NullReferenceException: Object reference not set to an instance of an object EventTester.OnEnable ()
        // (at Assets/Scripts/Tests/EventTester.cs:9)
        Debug.Log($"EVENT TEST: Player registered: {player.name}");
    }

    private void OnPlayerDeath()
    {
        Debug.Log("EVENT TEST: Player died!");
    }

    private void OnGameRestart()
    {
        Debug.Log("EVENT TEST: Game restarting...");
    }
}
