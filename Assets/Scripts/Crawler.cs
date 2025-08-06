using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Crawler : MonoBehaviour
{
    [SerializeField] private float _crawlSpeed = 3f;
    [SerializeField] private float _crawlHeight = 1f;
    [SerializeField] private Vector3 _crawlScale = new Vector3(1f, 0.5f, 1f);

    private CapsuleCollider _collider;
    private Vector3 _normalScale;
    private float _normalHeight;
    private bool _isCrawling;

    public bool IsCrawling => _isCrawling;
    public float CurrentSpeed => _isCrawling ? _crawlSpeed : 0f;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _normalScale = transform.localScale;
        _normalHeight = _collider.height;
    }

    public void DuckDown()
    {
        if (!_isCrawling)
        {
            _isCrawling = true;
            transform.localScale = _crawlScale;
            _collider.height = _crawlHeight;
        }
    }

    public void Standup()
    {
        if (_isCrawling)
        {
            _isCrawling = false;
            transform.localScale = _normalScale;
            _collider.height = _normalHeight;
        }
    }
}