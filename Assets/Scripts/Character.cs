using UnityEngine;

[RequireComponent(typeof(Userinput), typeof(CharacterMovement), typeof(GroundDetector))]
[RequireComponent(typeof(Jumper))]
public class Character : MonoBehaviour
{// 2
    [SerializeField] private Userinput _input;
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private GroundDetector _groundDetector;

    [SerializeField] private bool _isThief = false;
    // для тестов
    public bool IsThief => _isThief;

    private void Awake()
    {
        _input = GetComponent<Userinput>();
        _movement = GetComponent<CharacterMovement>();
        _jumper = GetComponent<Jumper>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void FixedUpdate()
    {
        if (_input.HorizontalDirection != 0 || _input.VerticalDirection != 0)
            _movement.Move(_input.HorizontalDirection, _input.VerticalDirection);

        if (_input.GetIsJump() && _groundDetector.IsGround)
            _jumper.Jump();
    }
}
