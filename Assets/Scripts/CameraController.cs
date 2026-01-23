using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -10);
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (_player == null)
            return;

        Vector3 desiredPosition = _player.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position,
                                                desiredPosition,
                                                _smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
        transform.LookAt(_player);
    }
}