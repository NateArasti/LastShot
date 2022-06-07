using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalApplicationSettings : MonoBehaviour
{
    private static GlobalApplicationSettings _instance;
    private static float soundVolume = 1;
    private static float musicVolume = 1;
    private static float brightness = 1;
    private static bool fullScreen = true;
    private static Resolution screenResolution = Resolution._1920x1080;
    private const string SoundVolumeKey = "Sound";
    private const string MusicVolumeKey = "Music";
    private const string BrightnessKey = "Brightness";
    private const string FullscreenKey = "Fullscreen";
    private const string ResolutionKey = "Resolution";

    [SerializeField] private VolumeProfile volumeProfile;

    public enum Resolution
    {
        _3840x2160,
        _2560x1440,
        _1920x1080,
        _1280x1080
    }

    public static float SoundVolume
    {
        get => soundVolume;
        set
        {
            value = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
            soundVolume = value;
        }
    }
    public static float MusicVolume
    {
        get => musicVolume;
        set
        {
            value = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
            musicVolume = value;
        }
    }
    public static float Brightness
    {
        get => brightness;
        set
        {
            value = Mathf.Clamp01(value);
            if(_instance.volumeProfile.TryGet(out LiftGammaGain liftGammaGain))
            {
                liftGammaGain.lift.value = new Vector4(0, 0, 0, value - 0.5f);
            }
            PlayerPrefs.SetFloat(BrightnessKey, value);
            brightness = value;
        }
    }
    public static bool FullScreen
    {
        get => fullScreen;
        set
        {
            PlayerPrefs.SetInt(ResolutionKey, value ? 1 : 0);
            Screen.fullScreen = value;
            Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            fullScreen = value;
        }
    }
    public static Resolution ScreenResolution
    {
        get => screenResolution; 
        set
        {
            PlayerPrefs.SetInt(ResolutionKey, (int) value);
            switch (screenResolution)
            {
                case Resolution._3840x2160:
                    Screen.SetResolution(3840, 2160, fullScreen);
                    break;
                case Resolution._2560x1440:
                    Screen.SetResolution(2560, 1440, fullScreen);
                    break;
                case Resolution._1920x1080:
                    Screen.SetResolution(1920, 1080, fullScreen);
                    break;
                case Resolution._1280x1080:
                    Screen.SetResolution(1280, 1080, fullScreen);
                    break;
            }
            screenResolution = value;
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
        LoadAllSettings();
    }

    public static void QuitGame() => Application.Quit();

    private void LoadAllSettings()
    {
        SoundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1);
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1);
        Brightness = PlayerPrefs.GetFloat(BrightnessKey, 1);
        FullScreen = PlayerPrefs.GetInt(FullscreenKey, 1) == 1 ? true : false;
        ScreenResolution = (Resolution)PlayerPrefs.GetInt(ResolutionKey, 2);
    }
}
