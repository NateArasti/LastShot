using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _quitGameButton;

    private void Awake()
    {
        _quitGameButton.onClick.AddListener(GlobalApplicationSettings.QuitGame);
    }
}
