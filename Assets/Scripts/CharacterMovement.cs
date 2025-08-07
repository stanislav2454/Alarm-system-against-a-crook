using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Runner), typeof(Crawler))]
public class CharacterMovement : MonoBehaviour
{
    private const int ZeroValue = 0;

    [SerializeField] private MovementSettings _settings;

   // private float _currentMoveSpeed;
    private Runner _runner;
    private Crawler _crawler;

    private void Awake()
    {
        _runner = GetComponent<Runner>();
        _crawler = GetComponent<Crawler>();
    }

    public void Move(float horizontalDirection, float verticalDirection)
    {// - не надо передавать данные между методами через поля.
     //   Для этого есть входные параметры и возвращаемое значение.
        float _currentMoveSpeed = GetCurrentSpeed();

        Vector3 direction = new Vector3(horizontalDirection, ZeroValue, verticalDirection) * _currentMoveSpeed * Time.deltaTime;
        transform.Translate(direction);
    }

    private float GetCurrentSpeed()
    {
        if (_crawler.IsCrawling)
            return _settings.CrawlSpeed;

        if (_runner.IsRunning)
            return _settings.RunSpeed;

        return _settings.WalkSpeed;
    }
}
