using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 2f;
    [SerializeField] private float _maxVolume = 1f;

    private AudioSource _alarm;
    private Coroutine _volumeCoroutine;
    private float _minVolume = 0f;

    private void Awake()
    {
        _alarm = GetComponent<AudioSource>();
        _alarm.volume = _minVolume;
        _alarm.loop = true;
    }

    public void Activate()
    {
        ChangeVolume(_maxVolume);

        if (_alarm.isPlaying == false)
            _alarm.Play();
    }

    public void Deactivate()
    {
        ChangeVolume(_minVolume);
    }

    private void ChangeVolume(float targetVolume)
    {
        if (_volumeCoroutine != null)
            StopCoroutine(_volumeCoroutine);

        _volumeCoroutine = StartCoroutine(VolumeFading(targetVolume));
    }

    private IEnumerator VolumeFading(float targetVolume)
    {
        while (Mathf.Approximately(_alarm.volume, targetVolume) == false)
        {
            _alarm.volume = Mathf.MoveTowards(
                _alarm.volume,
                targetVolume,
                Time.deltaTime / _fadeDuration);

            yield return null;
        }

        if (Mathf.Approximately(_alarm.volume, _minVolume))
            _alarm.Stop();
    }
}