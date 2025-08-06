using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Userinput : MonoBehaviour
{
    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);

    [SerializeField] private CharacterMovement _movement;
    //private readonly string MouseX = ("Mouse X");
    //public readonly string MouseY = ("Mouse Y");

    //[SerializeField] private float _rotateSpeed = 90;
    //[SerializeField] private float _maxAngle = 20f;
    //[SerializeField] private float _minAngle = -40f;
    //[SerializeField] private Transform _camera;
    //private float _cameraVertScroll;


    //[SerializeField] private float _walkSpeed = 5;
    //[SerializeField] private float _runSpeed = 10;
    //[SerializeField] private float _jumpPower = 10;
    // [SerializeField] private Transform _body;

    //private float _moveSpeed;
    //private Rigidbody _rigidbody;

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }

    //private void Awake()
    //{
    //    _rigidbody = GetComponent<Rigidbody>();
    //    _moveSpeed = _walkSpeed;
    //}


    //public const string Horizontal = "Horizontal";

    //private bool _isJump;

    //public float Direction { get; private set; }

    //private void Update()
    //{
    //    Direction = Input.GetAxis(Horizontal);

    //    if (Input.GetKeyDown(KeyCode.W))
    //        _isJump = true;
    //}

    //public bool GetIsJump() => GetBoolAsTrigger(ref _isJump);

    //private bool GetBoolAsTrigger(ref bool value)
    //{
    //    bool localValue = value;
    //    value = false;
    //    return localValue;
    //}
    private void Update()
    {
        Move();
        Jump();
        DuckDown();
        Run();
    }

    private void DuckDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _movement.DuckDown();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _movement.Standup();
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movement.Run();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _movement.Walk();
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movement.Jump();
        }
    }

    private void Move()
    {
        HorizontalDirection = Input.GetAxis(Horizontal);
        VerticalDirection = Input.GetAxis(Vertical);

       // _movement.Move(HorizontalDirection, VerticalDirection);
    }
}