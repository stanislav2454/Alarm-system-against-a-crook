using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmController : MonoBehaviour
{
    [SerializeField] private AudioSource _alarmSound;
    [SerializeField] private float _fadeSpeed = 0.5f;

    private float _minVolume = 0f;
    private float _maxVolume = 1f;
    private float _targetVolume = 0f;
    private bool _isThiefInside = false;

    private void Start()
    {
        _alarmSound.volume = _minVolume;
    }

    private void Update()
    {
        _alarmSound.volume = Mathf.MoveTowards(
            _alarmSound.volume,
            _targetVolume,
            _fadeSpeed * Time.deltaTime);

        if (_alarmSound.volume == _minVolume && _isThiefInside == false)
            _alarmSound.Stop();
        else if (_alarmSound.volume > _minVolume && _alarmSound.isPlaying == false)
            _alarmSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character character))
        {
            if (character.IsThief)// для тестов
            {
                _isThiefInside = true;
                _targetVolume = _maxVolume;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Character>(out _))
        {
            _isThiefInside = false;
            _targetVolume = _minVolume;
        }
    }
}