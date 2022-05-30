using UnityEngine;
using UnityEngine.Events;

public class GameTimeController : MonoBehaviour
{
    private static GameTimeController _instance;

    /// <summary>
    /// Value between 0 and 1, where 0 - day, 1 - night
    /// </summary>
    public static float CurrentTime { get; private set; }
    public static bool Paused { get; set; } = false;
    public static float TimeScale => _instance._gameHoursPerDay / _instance._realHoursPerDay;
    public static UnityEvent OnDayEnd { get; } = new();

    [SerializeField] private float _realHoursPerDay;
    [SerializeField] private float _gameHoursPerDay;
    private float _gameSecondsPerDay;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        CurrentTime = 0;
        _gameSecondsPerDay = _gameHoursPerDay * 3600;
    }

    private void Update()
    {
        if (Paused)
        {
            CurrentTime += Time.deltaTime / _gameSecondsPerDay;
        }
        else
        {
            CurrentTime += (Time.deltaTime * TimeScale) / _gameSecondsPerDay;
        }

        CurrentTime = Mathf.Clamp01(CurrentTime);
        if (Mathf.Approximately(CurrentTime, 1))
        {
            OnDayEnd.Invoke();
        }
    }
}
