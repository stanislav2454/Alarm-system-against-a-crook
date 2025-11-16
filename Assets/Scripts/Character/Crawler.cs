using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Crawler : MonoBehaviour
{
    [SerializeField] private float _crawlHeight = 1f;
    [SerializeField] private Vector3 _crawlScale = new Vector3(1f, 0.5f, 1f);

    private CapsuleCollider _collider;
    private Vector3 _normalScale;
    private float _normalHeight;
    private bool _isCrawling;

    public bool IsCrawling => _isCrawling;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _normalScale = transform.localScale;
        _normalHeight = _collider.height;
    }

    public void SetCrawling(bool crawl)
    {
        if (_isCrawling == crawl) 
            return;

        _isCrawling = crawl;
        transform.localScale = crawl ? _crawlScale : _normalScale;
        _collider.height = crawl ? _crawlHeight : _normalHeight;
    }
}