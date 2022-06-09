using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private Button _menuButton;

    private void Awake()
    {
        _quitGameButton.onClick.AddListener(GlobalApplicationSettings.QuitGame);
        _menuButton.onClick.AddListener(() =>
        {
            GameStateManager.UnPause();
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu, LoadSceneMode.Single);
        });
    }
}
