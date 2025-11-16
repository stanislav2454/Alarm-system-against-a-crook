using System;
using UnityEngine;

public class AlarmZone : MonoBehaviour
{
    public event Action<Thief> ThiefEntered;
    public event Action ThiefExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Thief thief) && thief.IsThief)
            ThiefEntered?.Invoke(thief);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Thief thief) && thief.IsThief)
            ThiefExited?.Invoke();
    }
}