using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayEndPanel : MonoBehaviour
{
    [SerializeField] private Button _exitGameButton;
    [SerializeField] private Button _exitToMenuButton;
    [SerializeField] private TMP_Text _ordersText;

    private void Awake()
    {
        _exitGameButton.onClick.AddListener(GlobalApplicationSettings.QuitGame);
        _exitToMenuButton.onClick.AddListener(() =>
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu, LoadSceneMode.Single));
    }

    private void OnEnable()
    {
        _ordersText.text = PlayerStorage.MoneyData.EarnedData();
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
