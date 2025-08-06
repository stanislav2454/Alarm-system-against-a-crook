using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Userinput), typeof(CharacterMovement), typeof(GroundDetector))]
public class Character : MonoBehaviour
{
    [SerializeField] private Userinput _input;
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private GroundDetector _groundDetector;

    private void Awake()
    {
        _input = GetComponent<Userinput>();
        _movement = GetComponent<CharacterMovement>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void FixedUpdate()
    {
        if (_input.HorizontalDirection != 0 || _input.VerticalDirection != 0)
            _movement.Move(_input.HorizontalDirection, _input.VerticalDirection);
    }
    //private GroundDetector _groundDetector;
    //private InputReader _inputReader;
    //private Mover _mover;

    //private void FixedUpdate()
    //{
    //    if (_inputReader.Direction != 0)
    //        _mover.Move(_inputReader.Direction);

    //    if (_inputReader.GetIsJump() && _groundDetector.IsGround)
    //        _mover.Jump();
    //}
}
