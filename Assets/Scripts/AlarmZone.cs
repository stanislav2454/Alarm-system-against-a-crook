using UnityEngine;
using UnityEngine.Events;

public class AlarmZone : MonoBehaviour
{
    public event UnityAction<Character> ThiefEntered;
    public event UnityAction ThiefExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out var character) && character.IsThief)
        {
            ThiefEntered?.Invoke(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Character>(out var character) && character.IsThief)
        {
            ThiefExited?.Invoke();
        }
    }
}