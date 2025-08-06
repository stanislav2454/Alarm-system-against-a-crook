using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 6;
    [SerializeField] private float _runSpeed = 10;
    [SerializeField] private float _crawlSpeed = 3;
    [SerializeField] private float _jumpPower = 5;

    private float _moveSpeed;
    private bool _isRun;
    private bool _ifSitt;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _moveSpeed = _walkSpeed;
    }

    public void Move(float HorizontalDirection, float VerticalDirection)
    {
        Vector3 direction = new Vector3(HorizontalDirection, 0, VerticalDirection) * _moveSpeed * Time.deltaTime;
        transform.Translate(direction);
    }

    public void Run()
    {// todo Lerp ?
        if (_ifSitt == false)
        {
            _isRun = true;
            _moveSpeed = _runSpeed;
        }
    }

    public void Walk()
    {// todo Lerp ?
        _isRun = false;

        if (_ifSitt == true)
        {
            _moveSpeed = _crawlSpeed;
        }
        else
        {
            _moveSpeed = _walkSpeed;
        }
    }

    #region DuckDown
    public void DuckDown()
    {// todo Lerp ?
        if (_isRun == false)
        {
            _ifSitt = true;
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            _collider.height = 1f;
            //transform.position = new Vector3(transform.position.x, -0.2f, transform.position.z);
            _moveSpeed = _crawlSpeed;
        }
    }

    public void Standup()
    {// todo Lerp ?
        _ifSitt = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
        _collider.height = 2f;
        _moveSpeed = _walkSpeed;
    }
    #endregion

    public void Jump()
    {
        //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        //_rigidbody.AddForce(new Vector2(0, _jumpForce));
        _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
    }
    //private float _speedX;
    //private float _jumpForce;
    //private Rigidbody2D _rigidbody;

    //public void Jump()
    //{
    //    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
    //    _rigidbody.AddForce(new Vector2(0, _jumpForce));
    //}

    //public void Move(float direction)
    //{
    //    _rigidbody.velocity = new Vector2(_speedX * direction * Time.fixedDeltaTime, _rigidbody.velocity.y);
    //}
}
