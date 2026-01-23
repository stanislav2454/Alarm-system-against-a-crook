using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(Runner), typeof(Crawler))]
public class Userinput : MonoBehaviour
{
    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);
    private const string MouseX = ("Mouse X");
    private const string MouseY = ("Mouse Y");

    [Header("Input Settings")]
    [SerializeField] private KeyCode _attackButton = KeyCode.Mouse0;

    [SerializeField] private Runner _runner;
    [SerializeField] private Crawler _crawler;

    private bool _isJump;

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }
    public string HorizontalMouseDirection { get; private set; }
    public string VerticalMouseDirection { get; private set; }
    public bool AttackInput { get; private set; }
    public bool IsReloading { get; private set; }

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
        _runner.SetRunning(Input.GetKey(KeyCode.LeftShift));

    private void Attack() =>
        AttackInput = Input.GetKey(_attackButton);
    //AttackInput = Input.GetKeyDown(_attackButton);

    private void ReloadWeapon() =>
        IsReloading = Input.GetKeyDown(KeyCode.R);

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_runner.IsRunning)
                _runner.SetRunning(false);

            _crawler.SetCrawling(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _crawler.SetCrawling(false);
        }
    }
}