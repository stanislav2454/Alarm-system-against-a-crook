using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Runner), typeof(Crawler))]
public class CharacterMovement : MonoBehaviour
{
    private const int ZeroValue = 0;

    [SerializeField] private float _walkSpeed = 6;

    private float _currentMoveSpeed;
    private Runner _runner;
    private Crawler _crawler;

    private void Awake()
    {
        _runner = GetComponent<Runner>();
        _crawler = GetComponent<Crawler>();
    }

    public void Move(float horizontalDirection, float verticalDirection)
    {
        UpdateMovementSpeed();

        Vector3 direction = new Vector3(horizontalDirection, ZeroValue, verticalDirection) * _currentMoveSpeed * Time.deltaTime;
        transform.Translate(direction);
    }

    private void UpdateMovementSpeed()
    {
        if (_crawler.IsCrawling)
        {
            _currentMoveSpeed = _crawler.CurrentSpeed;
        }
        else if (_runner.IsRunning)
        {
            _currentMoveSpeed = _runner.CurrentSpeed;
        }
        else
        {
            _currentMoveSpeed = _walkSpeed;
        }
    }
}
