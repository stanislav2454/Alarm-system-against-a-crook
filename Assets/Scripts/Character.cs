using UnityEngine;

[RequireComponent(typeof(Userinput), typeof(MouseLooking), typeof(CharacterMovement))]
[RequireComponent(typeof(Jumper), typeof(GroundDetector))]
public class Character : MonoBehaviour
{
    [SerializeField] private Userinput _input;
    [SerializeField] private MouseLooking _mouseLooking;
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Thief _thiefIdentity;

    //[Space(3)]
    //[SerializeField] private bool _isThief = false;

    // public bool IsThief => _isThief;

    private void Awake()
    {
        _input = GetComponent<Userinput>();
        _mouseLooking = GetComponent<MouseLooking>();
        _movement = GetComponent<CharacterMovement>();
        _jumper = GetComponent<Jumper>();
        _groundDetector = GetComponent<GroundDetector>();
        _thiefIdentity = GetComponent<Thief>();
    }

    private void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        HandleJump();
    }

    private void HandleRotation()
    {
        if (_input.HorizontalMouseDirection != null || _input.VerticalMouseDirection != null)
            _mouseLooking.Rotate(_input.HorizontalMouseDirection, _input.VerticalMouseDirection);
    }

    private void HandleMovement()
    {
        if (_input.HorizontalDirection != 0 || _input.VerticalDirection != 0)
            _movement.Move(_input.HorizontalDirection, _input.VerticalDirection);
    }

    private void HandleJump()
    {
        if (_input.GetIsJump() && _groundDetector.IsGrounded)
            _jumper.Jump();
    }
}
