using UnityEngine;

[RequireComponent(typeof(AlarmZone), typeof(AlarmSystem))]
public class AlarmTrigger : MonoBehaviour
{
    [SerializeField] private AlarmZone _zone;
    [SerializeField] private AlarmSystem _alarm;

    private void Awake()
    {
        _zone = GetComponent<AlarmZone>();
        _alarm = GetComponent<AlarmSystem>();

    }

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

    private void OnThiefEntered(Thief thief) =>
        _alarm.Activate();

    private void OnThiefExited() =>
        _alarm.Deactivate();
}