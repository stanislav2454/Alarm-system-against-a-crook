using UnityEngine;

public class MouseLooking : MonoBehaviour
{
    private const int ZeroValue = 0;

    [SerializeField] private float _rotateSpeed = 90;
    [SerializeField] private float _maxAngle = 20f;
    [SerializeField] private float _minAngle = -40f;
    [SerializeField] private Transform _camera;

    private float _cameraVertScroll;

    public void Rotate(string mouseX, string mouseY)
    {
        _cameraVertScroll -= Input.GetAxis(mouseY) * _rotateSpeed * Time.deltaTime;
        _cameraVertScroll = Mathf.Clamp(_cameraVertScroll, _minAngle, _maxAngle);
        _camera.localEulerAngles = new Vector3(_cameraVertScroll, ZeroValue, ZeroValue);

        transform.Rotate(Input.GetAxis(mouseX) * _rotateSpeed * Time.deltaTime * Vector3.up);
    }
}
