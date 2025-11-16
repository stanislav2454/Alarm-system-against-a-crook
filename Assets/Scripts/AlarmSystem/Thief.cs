using UnityEngine;

public class Thief : MonoBehaviour
{
    [SerializeField] private bool _isThief;

    public bool IsThief => _isThief;

    public void SetThiefStatus(bool isThief) => 
        _isThief = isThief;
}
