using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _soundGroup;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public static void PlaySound(AudioClip clip, float pitchRandomRange = 0, bool playExclusivly = false)
    {
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
        _instance._soundGroup.audioMixer.SetFloat("SoundVolume", Mathf.Lerp(-80, 0, volume));
    }

    public static void ChangeMusicVolume(float volume)
    {
        _instance._musicGroup.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, volume));
    }
}
