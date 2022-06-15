using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        MainMenu,
        Game,
        Tutorial
    }

    private static SceneLoader _instance;
    private static float _currentLoadValue;
    [SerializeField] private CanvasGroup _loadScreen;
    [SerializeField] private Image _loadSlider;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("SceneLoader already exists!");
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _loadScreen.gameObject.SetActive(false);
    }

    public static async void LoadScene(string sceneName, LoadSceneMode mode)
    {
        _instance.EnableLoadScreen();
        var sceneLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        sceneLoad.allowSceneActivation = false;

        do
        {
            await Task.Delay(500);
            _currentLoadValue = sceneLoad.progress;
        } while (sceneLoad.progress < 0.9f);
        sceneLoad.allowSceneActivation = true;
    }

    public static async void UnloadScene(string sceneName)
    {
        _instance.EnableLoadScreen();
        var sceneLoad = SceneManager.UnloadSceneAsync(sceneName);
        sceneLoad.allowSceneActivation = false;

        do
        {
            await Task.Delay(1000);
            _currentLoadValue = sceneLoad.progress;
        } while (sceneLoad.progress < 0.9f);
        sceneLoad.allowSceneActivation = true;
    }

    public static void LoadScene(Scene scene, LoadSceneMode mode) => LoadScene(scene.ToString(), mode);
    public static void UnloadScene(Scene scene) => UnloadScene(scene.ToString());

    private void EnableLoadScreen()
    {
        _loadSlider.fillAmount = 0;
        StartCoroutine(enumerator());

        IEnumerator enumerator()
        {
            _loadScreen.gameObject.SetActive(true);
            _loadScreen.alpha = 0;
            while (!Mathf.Approximately(_loadScreen.alpha, 1))
            {
                _loadScreen.alpha = Mathf.MoveTowards(
                    _loadScreen.alpha,
                    1,
                    10 * Time.deltaTime);
                yield return null;
            }
            while (_loadSlider.fillAmount < 0.9f)
            {
                _loadSlider.fillAmount = Mathf.MoveTowards(
                    _loadSlider.fillAmount,
                    _currentLoadValue,
                    Time.deltaTime);
                yield return null;
            }
            while (!Mathf.Approximately(_loadScreen.alpha, 0))
            {
                _loadScreen.alpha = Mathf.MoveTowards(
                    _loadScreen.alpha,
                    0,
                    10 * Time.deltaTime);
                yield return null;
            }
            _loadScreen.gameObject.SetActive(false);
        }
    }
}
