using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Userinput : MonoBehaviour
{// 1
    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);

    [SerializeField] private CharacterMovement _movement;

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }

    private void Update()
    {
        Move();

        //====todo
        Jump();
        DuckDown();
        Run();
    }

    private void Move()
    {
        HorizontalDirection = Input.GetAxis(Horizontal);
        VerticalDirection = Input.GetAxis(Vertical);
    }

    //====todo
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
}