using UnityEngine;

public class GlobalApplicationSettings : MonoBehaviour
{
    private static GlobalApplicationSettings _instance;

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

    public static void QuitGame() => Application.Quit();
}
