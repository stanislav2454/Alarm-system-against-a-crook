using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool IsGround { get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
            IsGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
            IsGround = false;
    }
}
