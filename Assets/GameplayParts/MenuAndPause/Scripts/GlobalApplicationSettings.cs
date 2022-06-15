using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalApplicationSettings : MonoBehaviour
{
    private static GlobalApplicationSettings _instance;
    private static float _soundVolume = 1;
    private static float _musicVolume = 1;
    private static float _brightness = 0.5f;
    private static bool _fullScreen = true;
    private static Resolution _screenResolution = Resolution._1920x1080;
    private const string SoundVolumeKey = "Sound";
    private const string MusicVolumeKey = "Music";
    private const string BrightnessKey = "Brightness";
    private const string FullscreenKey = "Fullscreen";
    private const string ResolutionKey = "Resolution";

    [SerializeField] private VolumeProfile _volumeProfile;

    public enum Resolution
    {
        _3840x2160,
        _2560x1440,
        _1920x1080,
        _1280x1080
    }

    public static float SoundVolume
    {
        get => _soundVolume;
        set
        {
            value = Mathf.Clamp01(value);
            AudioManager.ChangeSoundVolume(value);
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
            _soundVolume = value;
        }
    }
    public static float MusicVolume
    {
        get => _musicVolume;
        set
        {
            value = Mathf.Clamp01(value);
            AudioManager.ChangeMusicVolume(value);
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
            _musicVolume = value;
        }
    }
    public static float Brightness
    {
        get => _brightness;
        set
        {
            value = Mathf.Clamp01(value);
            if(_instance._volumeProfile.TryGet(out LiftGammaGain liftGammaGain))
            {
                liftGammaGain.lift.value = new Vector4(0, 0, 0, value - 0.5f);
            }
            PlayerPrefs.SetFloat(BrightnessKey, value);
            _brightness = value;
        }
    }
    public static bool FullScreen
    {
        get => _fullScreen;
        set
        {
            PlayerPrefs.SetInt(ResolutionKey, value ? 1 : 0);
            Screen.fullScreen = value;
            Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            _fullScreen = value;
        }
    }
    public static Resolution ScreenResolution
    {
        get => _screenResolution; 
        set
        {
            PlayerPrefs.SetInt(ResolutionKey, (int) value);
            switch (_screenResolution)
            {
                case Resolution._3840x2160:
                    Screen.SetResolution(3840, 2160, _fullScreen);
                    break;
                case Resolution._2560x1440:
                    Screen.SetResolution(2560, 1440, _fullScreen);
                    break;
                case Resolution._1920x1080:
                    Screen.SetResolution(1920, 1080, _fullScreen);
                    break;
                case Resolution._1280x1080:
                    Screen.SetResolution(1280, 1080, _fullScreen);
                    break;
            }
            _screenResolution = value;
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("GlobalApplicationSettings already exists!");
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        LoadAllSettings();
    }

    public static void QuitGame() => Application.Quit();

    private void LoadAllSettings()
    {
        SoundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1);
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1);
        Brightness = PlayerPrefs.GetFloat(BrightnessKey, 0.5f);
        FullScreen = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;
        ScreenResolution = (Resolution)PlayerPrefs.GetInt(ResolutionKey, 2);
    }

    public static void SetDefaultSettings()
    {
        SoundVolume = 1;
        MusicVolume = 1;
        Brightness = 0.5f;
        FullScreen = true;
        ScreenResolution = (Resolution)2;
    }
}
