using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(Runner), typeof(Crawler))]
public class Userinput : MonoBehaviour
{
    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);
    private readonly string MouseX = ("Mouse X");
    private readonly string MouseY = ("Mouse Y");

    [SerializeField] private Runner _runner;
    [SerializeField] private Crawler _crawler;

    private bool _isJump;

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }
    public string HorizontalMouseDirection { get; private set; }
    public string VerticalMouseDirection { get; private set; }
    public bool GetIsJump() => GetBoolAsTrigger(ref _isJump);

    private void Update()
    {
        Move();
        Jump();
        MouseLook();
        DuckDown();
        Run();
    }

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

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            _runner.Run();
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _runner.Walk();
    }

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
            _crawler.DuckDown();
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            _crawler.Standup();
    }
}