using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private Slider _soundVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _brightness;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private TMP_Dropdown _resolution;

    private void Awake()
    {
        _soundVolume.onValueChanged.AddListener(ChangeSoundVolume);
        _musicVolume.onValueChanged.AddListener(ChangeMusicVolume);
        _brightness.onValueChanged.AddListener(ChangeBrightness);
        _fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);
        _resolution.onValueChanged.AddListener(ChangeResolution);
    }

    private void OnEnable()
    {
        _soundVolume.value = GlobalApplicationSettings.SoundVolume;
        _musicVolume.value = GlobalApplicationSettings.MusicVolume;
        _brightness.value = GlobalApplicationSettings.Brightness;
        _fullscreenToggle.isOn = GlobalApplicationSettings.FullScreen;
        _resolution.value = (int)GlobalApplicationSettings.ScreenResolution;
    }

    public void ChangeSoundVolume(float volume)
    {
        GlobalApplicationSettings.SoundVolume = volume;
    }

    public void ChangeMusicVolume(float volume)
    {
        GlobalApplicationSettings.MusicVolume = volume;
    }

    public void ChangeBrightness(float brightness)
    {
        GlobalApplicationSettings.Brightness = brightness;
    }

    public void ToggleFullscreen(bool fullscreen)
    {
        GlobalApplicationSettings.FullScreen = fullscreen;
    }

    public void ChangeResolution(int resolutionIndex)
    {
        GlobalApplicationSettings.ScreenResolution = (GlobalApplicationSettings.Resolution) resolutionIndex;
    }

    public void ResetSettings()
    {
        GlobalApplicationSettings.SetDefaultSettings();
        OnEnable();
    }
}
