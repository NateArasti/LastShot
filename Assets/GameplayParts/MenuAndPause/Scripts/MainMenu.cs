using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Guest")]
    [SerializeField] private Guest _guest;
    [SerializeField] private Transform _sitSpot;
    [Header("Menu")]
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _credits;

    private void Start()
    {
        _guest.MoveToSpot(_sitSpot, Guest.SpotType.Chair, false, false);
    }

    public void LoadGame() => SceneLoader.LoadScene(SceneLoader.Scene.Game, LoadSceneMode.Single);

    public void LoadTutorial() => SceneLoader.LoadScene(SceneLoader.Scene.Tutorial, LoadSceneMode.Additive);

    public void ToggleSettings()
    {
        _settings.SetActive(!_settings.activeSelf);
        if(_settings.activeSelf) _credits.SetActive(false);
    }

    public void ToggleCredits()
    {
        _credits.SetActive(!_credits.activeSelf);
        if (_credits.activeSelf) _settings.SetActive(false);
    }

    public void QuitGame() => GlobalApplicationSettings.QuitGame();
}
