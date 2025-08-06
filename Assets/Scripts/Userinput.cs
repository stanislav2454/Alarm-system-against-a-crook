using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class Userinput : MonoBehaviour
{// 1
    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);

    [SerializeField] private CharacterMovement _movement;

    private bool _isJump;

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }
    public bool GetIsJump() => GetBoolAsTrigger(ref _isJump);

    private void Update()
    {
        Move();
        Jump();

        //====todo 
        DuckDown();
        Run();
    }

    private void Move()
    {
        HorizontalDirection = Input.GetAxis(Horizontal);
        VerticalDirection = Input.GetAxis(Vertical);
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

    //====== todo =======================================
    private void DuckDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            _movement.DuckDown();
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            _movement.Standup();
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            _movement.Run();
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _movement.Walk();
    }
}