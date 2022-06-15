using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _soundGroup;

    [Space(10f)] [SerializeField] private AudioClip[] _musicClips;
    private bool _paused;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => !_musicSource.isPlaying);
            _musicSource.clip = _musicClips.GetRandomObject();
            _musicSource.Play();
        }
    }

    public static void PlaySound(AudioClip clip, float pitchRandomRange = 0, bool playExclusivly = false, float volume = 1)
    {
        print(clip.name);
        AudioSource source;
        if (_instance._soundSource.isPlaying && !playExclusivly)
        {
            source = Instantiate(_instance._soundSource, _instance._soundSource.transform.parent);
            _instance.StartCoroutine(destroySourceEnumerator());
        }
        else
            source = _instance._soundSource;
        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(1 - pitchRandomRange, 1 + pitchRandomRange);
        source.Play();

        IEnumerator destroySourceEnumerator()
        {
            yield return new WaitUntil(() => !source.isPlaying);
            Destroy(source.gameObject);
        }
    }

    public static void ChangeSoundVolume(float volume)
    {
        _instance._soundGroup.audioMixer.SetFloat("SoundVolume", Mathf.Lerp(-60, 0, volume));
    }

    public static void ChangeMusicVolume(float volume)
    {
        _instance._musicGroup.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-60, 0, volume));
    }
}
