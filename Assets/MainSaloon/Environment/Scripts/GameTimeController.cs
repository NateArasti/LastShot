using System.Collections.Generic;
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
    private readonly HashSet<GameTimer> _timers = new();

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

        _timers.ForEachAction(timer => timer.Update());
        _timers.RemoveWhere(timer => timer.Ended);
    }

    public static GameTimer SetTimer(float waitTime)
    {
        var endTime = CurrentTime + TimeScale * waitTime / _instance._gameSecondsPerDay;
        var timer = new GameTimer(endTime);
        _instance._timers.Add(timer);
        return timer;
    }

    public class GameTimer
    {
        public bool Ended { get; private set; }
        private readonly float _targetTime;

        /// <summary>
        /// Don't use it
        /// Use SetTimer(float waitTime) instead
        /// </summary>
        /// <param name="endTime"></param>
        public GameTimer(float endTime)
        {
            _targetTime = endTime;
        }

        public void Update()
        {
            if (Mathf.Abs(_targetTime - CurrentTime) < float.Epsilon)
            {
                Ended = true;
            }
        }
    }
}
