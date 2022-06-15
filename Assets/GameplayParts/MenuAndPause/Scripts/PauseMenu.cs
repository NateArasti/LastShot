using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _tutorialButton;

    private void Awake()
    {
        _quitGameButton.onClick.AddListener(GlobalApplicationSettings.QuitGame);
        _tutorialButton.onClick.AddListener(() => SceneLoader.LoadScene(SceneLoader.Scene.Tutorial, LoadSceneMode.Additive));
        _menuButton.onClick.AddListener(() =>
        {
            GameStateManager.UnPause();
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu, LoadSceneMode.Single);
        });
    }
}
