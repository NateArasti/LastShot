using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private PageSwitch _pageSwitch;
    [SerializeField] private GameObject _rightSwitch;
    [SerializeField] private GameObject _leftSwitch;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public void CheckSwitchButtons()
    {
        _rightSwitch.SetActive(true);
        _leftSwitch.SetActive(true);
        if (_pageSwitch.CurrentPageIndex == _pageSwitch.PagesCount - 1)
            _rightSwitch.SetActive(false);
        if (_pageSwitch.CurrentPageIndex == 0)
            _leftSwitch.SetActive(false);
    }

    public void EndTutorial()
    {
        SceneLoader.UnloadScene(SceneLoader.Scene.Tutorial);
    }
}
