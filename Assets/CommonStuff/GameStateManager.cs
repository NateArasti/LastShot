using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public enum State
    {
        Saloon,
        Dialogue,
        Notebook,
        BarShelf,
        WorkSpace,
        Pause
    }

    private static GameStateManager _instance;

    public static State CurrentState { get; private set; } = State.Saloon;
    private static State _lastNormalState;
    public static readonly UnityEvent<State> OnStateChanged = new();

    [SerializeField] private StateEvent _pauseStateEvent;
    [SerializeField] private StateEvent _notebookStateEvent;
    [SerializeField] private StateEvent[] _stateEvents;
    private readonly Dictionary<State, (UnityEvent turnOn, UnityEvent turnOff)> _stateEventsDictionary = new();

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("GameStateManager already exists!");
            Destroy(gameObject);
            return;
        }

        _instance = this;
        OnStateChanged.AddListener(InvokeLocalStateEvent);
        _stateEvents.ForEachAction(stateEvent => 
            _stateEventsDictionary.Add(stateEvent.State, (stateEvent.TurnOnEvent, stateEvent.TurnOffEvent)));
        _stateEventsDictionary[State.Saloon].turnOn.AddListener(() => GameTimeController.Paused = false);
        _stateEventsDictionary[State.Saloon].turnOff.AddListener(() => GameTimeController.Paused = true);
        SwitchToSaloon();
    }

    private void OnDestroy() => OnStateChanged.RemoveAllListeners();

    private void InvokeLocalStateEvent(State state)
    {
        _stateEventsDictionary[CurrentState].turnOff.Invoke();
        _lastNormalState = state;
        CurrentState = state;
        _stateEventsDictionary[CurrentState].turnOn.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == State.Pause) UnPause();
            else Pause();
        }
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        _instance._pauseStateEvent.TurnOnEvent.Invoke();
        if (CurrentState == State.Notebook) _lastNormalState = CurrentState;
        CurrentState = State.Pause;
    }

    public static void UnPause()
    {
        Time.timeScale = 1;
        _instance._pauseStateEvent.TurnOffEvent.Invoke();
        CurrentState = _lastNormalState;
        Debug.LogError(CurrentState);
    }

    public static void NotebookOn()
    {
        _instance._notebookStateEvent.TurnOnEvent.Invoke();
        CurrentState = State.Notebook;
    }

    public static void NotebookOff()
    {
        _instance._notebookStateEvent.TurnOffEvent.Invoke();
        CurrentState = _lastNormalState;
    }

    public static void SwitchToSaloon()
    {
        OnStateChanged.Invoke(State.Saloon);
    }

    public static void SwitchToDialogue()
    {
        OnStateChanged.Invoke(State.Dialogue);
    }

    [System.Serializable]
    private struct StateEvent
    {
        [SerializeField] private State _state;
        [SerializeField] private UnityEvent _turnOn;
        [SerializeField] private UnityEvent _turnOff;

        public State State => _state;
        public UnityEvent TurnOnEvent => _turnOn;
        public UnityEvent TurnOffEvent => _turnOff;
    }
}
