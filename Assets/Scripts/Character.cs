using UnityEngine;

[RequireComponent(typeof(Userinput), typeof(CharacterMovement), typeof(GroundDetector))]
public class Character : MonoBehaviour
{// 2
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
}
