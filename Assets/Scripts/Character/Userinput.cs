using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(Runner), typeof(Crawler))]
public class Userinput : MonoBehaviour// !!! не грамотно именовано , надо UserInput !!!
{
    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);
    private const string MouseX = ("Mouse X");
    private const string MouseY = ("Mouse Y");

    [Header("Input Settings")]
    [SerializeField] private KeyCode _attackButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode _reloadButton = KeyCode.R;
    [SerializeField] private KeyCode _runButton = KeyCode.LeftShift;
    [SerializeField] private KeyCode _sitButton = KeyCode.LeftControl;
    [SerializeField] private KeyCode _jumpButton = KeyCode.Space;
    [SerializeField] private KeyCode _pauseButton = KeyCode.Escape;

    [SerializeField] private Runner _runner;
    [SerializeField] private Crawler _crawler;

    private bool _isJump;

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }
    public string HorizontalMouseDirection { get; private set; }
    public string VerticalMouseDirection { get; private set; }
    public bool AttackInput { get; private set; }
    public bool IsReloading { get; private set; }
    public bool IsPausePressed => Input.GetKeyDown(_pauseButton);// Добавляем свойство для доступа из других классов

    private void Update()
    {
        Move();
        Jump();
        MouseLook();
        DuckDown();
        Run();
        Attack();
        ReloadWeapon();
    }

    public bool GetIsJump() =>
        GetBoolAsTrigger(ref _isJump);

    private void Move()
    {
        HorizontalDirection = Input.GetAxis(Horizontal);
        VerticalDirection = Input.GetAxis(Vertical);
    }

    private void MouseLook()
    {
        HorizontalMouseDirection = MouseX;
        VerticalMouseDirection = MouseY;
    }

    private void Run() =>
        _runner.SetRunning(Input.GetKey(_runButton));

    private void Attack() =>
        AttackInput = Input.GetKey(_attackButton);

    private void ReloadWeapon() =>
        IsReloading = Input.GetKeyDown(_reloadButton);

    private void Jump()
    {
        if (Input.GetKeyDown(_jumpButton))
            _isJump = true;
    }

    private bool GetBoolAsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;
        return localValue;
    }

    private void DuckDown()
    {
        if (Input.GetKeyDown(_sitButton))
        {
            if (_runner.IsRunning)
                _runner.SetRunning(false);

            _crawler.SetCrawling(true);
        }
        else if (Input.GetKeyUp(_sitButton))
        {
            _crawler.SetCrawling(false);
        }
    }
}