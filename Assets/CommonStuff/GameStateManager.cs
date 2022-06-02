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
    public static readonly UnityEvent<State> OnStateChanged = new(); 

    [SerializeField] private StateEvent[] _stateEvents = new StateEvent[6];
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
        _stateEventsDictionary[State.Pause].turnOn.AddListener(() => Time.timeScale = 0);
        _stateEventsDictionary[State.Pause].turnOff.AddListener(() => Time.timeScale = 1);
        _stateEventsDictionary[State.Saloon].turnOn.AddListener(() => GameTimeController.Paused = false);
        _stateEventsDictionary[State.Saloon].turnOff.AddListener(() => GameTimeController.Paused = true);
        SwitchToSaloon();
    }

    private void OnDestroy() => OnStateChanged.RemoveAllListeners();

    private void InvokeLocalStateEvent(State state)
    {
        _stateEventsDictionary[CurrentState].turnOff.Invoke();
        CurrentState = state;
        _stateEventsDictionary[CurrentState].turnOn.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Pause();
    }

    public static void Pause()
    {
        OnStateChanged.Invoke(State.Pause);
    }

    public static void SwitchToNotebook()
    {
        OnStateChanged.Invoke(State.Notebook);
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
