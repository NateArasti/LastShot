using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Guest")]
    [SerializeField] private Guest _guest;
    [SerializeField] private Transform _sitSpot;
    [Header("Scenes")] 
    [SerializeField] private string _gameSceneName = "Game";
    [SerializeField] private string _tutorialSceneName = "Tutorial";
    [Header("Menu")]
    [SerializeField] private GameObject _settings;

    private void Start()
    {
        _guest.MoveToSpot(_sitSpot, Guest.SpotType.Chair, false, false);
    }

    public void LoadGame() => SceneLoader.LoadScene(_gameSceneName, LoadSceneMode.Single);

    public void LoadTutorial() => SceneLoader.LoadScene(_tutorialSceneName, LoadSceneMode.Additive);

    public void ToggleSettings()
    {
        _settings.SetActive(!_settings.activeSelf);
    }

    public void QuitGame() => GlobalApplicationSettings.QuitGame();
}
