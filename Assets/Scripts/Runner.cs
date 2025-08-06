using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 10f;

    private bool _isRunning;

    public bool IsRunning => _isRunning;
    public float CurrentSpeed => _isRunning ? _runSpeed : 0f;

    public void Run()
    {
        _isRunning = true;
    }

    public void Walk()
    {
        _isRunning = false;
    }
}