using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    [SerializeField] private AlarmZone _zone;
    [SerializeField] private AlarmSystem _alarm;

    private void OnEnable()
    {
        _zone.ThiefEntered += OnThiefEntered;
        _zone.ThiefExited += OnThiefExited;
    }

    private void OnDisable()
    {
        _zone.ThiefEntered -= OnThiefEntered;
        _zone.ThiefExited -= OnThiefExited;
    }

    private void OnThiefEntered(Character thief)
    {
        _alarm.Activate();
    }

    private void OnThiefExited()
    {
        _alarm.Deactivate();
    }
}