using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLooking : MonoBehaviour
{

    private readonly string MouseX = ("Mouse X");
    public readonly string MouseY = ("Mouse Y");

    [SerializeField] private float _rotateSpeed = 90;
    [SerializeField] private float _maxAngle = 20f;
    [SerializeField] private float _minAngle = -40f;
    [SerializeField] private Transform _camera;

    private float _cameraVertScroll;

    //void Start()
    //{

    //}

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        _cameraVertScroll -= Input.GetAxis(MouseY) * _rotateSpeed * Time.deltaTime;
        _cameraVertScroll = Mathf.Clamp(_cameraVertScroll, _minAngle, _maxAngle);
        _camera.localEulerAngles = new Vector3(_cameraVertScroll, 0, 0);

        transform.Rotate(Input.GetAxis(MouseX) * _rotateSpeed * Time.deltaTime * Vector3.up);
        //_body.Rotate(Input.GetAxis(MouseX) * _rotateSpeed * Time.deltaTime * Vector3.up);
        //RotationY = Input.GetAxis("Mouse X");
        //transform.Rotate(RotationY * _rotateSpeed * Time.deltaTime * Vector3.up);



        //transform.Rotate(rotation * _rotateSpeed * Time.deltaTime * Vector3.up);

        //HorizontalDirection = Input.GetAxis(Horizontal);
        //transform.Rotate(HorizontalDirection * _rotateSpeed * Time.deltaTime * Vector3.up);
    }
}
