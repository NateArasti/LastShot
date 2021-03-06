using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueUI))]
public class DialogueSystem : MonoBehaviour
{
    public enum DialogueState
    {
        None,
        SimplePhrase,
        PlayerChoice,
        PlayerSuggest,
        TagState
    }

    public enum EventType
    {
        End,
        Order,
        Wait,
        GuestChoice,
        Check,
        Grade,
        PlayerSuggest
    }

    private static readonly KeyCode[] KeyCodesToSkip =
    {
        KeyCode.Space,
        KeyCode.E,
        KeyCode.Mouse0,
        KeyCode.Return
    };

    private static Dialogue _currentDialogue;

    /// <summary>
    /// If not null, can't set new
    /// </summary>
    public static Dialogue CurrentDialogue
    {
        get => _currentDialogue;
        set
        {
            if(_currentDialogue != null) return;
            _currentDialogue = value;
        }
    }

    private readonly UnityEvent<EventType> _dialogueEventInvoker = new();

    private readonly IReadOnlyDictionary<EventType, UnityEvent> _dialogueEvents =
        new Dictionary<EventType, UnityEvent>
        {
            [EventType.End] = new(),
            [EventType.PlayerSuggest] = new(),
            [EventType.Order] = new(),
            [EventType.Wait] = new(),
            [EventType.GuestChoice] = new(),
            [EventType.Check] = new(),
            [EventType.Grade] = new()
        };

    [SerializeField] private UnityEvent _playerSuggestEvent;
    private DialogueState _currentState;
    private DialogueState _nextState;
    private Story _currentStory;
    private DialogueUI _dialogueUI;

    private float _startDelay = 0.5f;

    private void Awake()
    {
        _dialogueUI = GetComponent<DialogueUI>();
        _dialogueEventInvoker.AddListener(type => _dialogueEvents[type].Invoke());

        _dialogueEvents[EventType.PlayerSuggest].AddListener(() =>
        {
            _playerSuggestEvent.Invoke();
        });
        _dialogueEvents[EventType.End].AddListener(() =>
        {
            CurrentDialogue.OnEnd.Invoke();
            _currentDialogue = null;
            _currentStory = null;
            _currentState = DialogueState.None;
            GameStateManager.SwitchToSaloon();
        });
        CreateTagEvents();
    }

    public void SuggestDrink(Drink drink)
    {
        _currentState = DialogueState.SimplePhrase;
        CurrentDialogue.CurrentOrderData.SuggestedDrink = drink;
        ShowNewPhrase(drink);
    }

    private void LateUpdate()
    {
        if (_startDelay > 0) _startDelay -= Time.deltaTime;
        if (_startDelay > 0 || GameStateManager.CurrentState != GameStateManager.State.Dialogue || _currentState != DialogueState.SimplePhrase || !KeyCodesToSkip.Any(Input.GetKeyDown)) return;
        if (_dialogueUI.IsTypingPhrase)
        {
            _dialogueUI.EndTyping();
        }
        else
        {
            if (_nextState != DialogueState.None)
            {
                _currentState = _nextState;
                _nextState = DialogueState.None;
            }
            UpdateDialogueState();
        }
    }

    public void StartDialogue()
    {
        if(CurrentDialogue == null || _currentState != DialogueState.None)
            throw new UnityException("Dialogue is already going!!!");
        CurrentDialogue.CurrentOrderData = new Dialogue.OrderData();
        _currentStory = new Story(CurrentDialogue.Text.text);
        UpdateDialogueState();
    }

    private void EndDialogue()
    {
        _dialogueEventInvoker.Invoke(EventType.End);
    }

    private void UpdateDialogueState(bool ignoreTags = false)
    {
        TryParseErrors();
        if (!ignoreTags && _currentStory.currentTags.Count > 0)
        {
            _currentState = DialogueState.TagState;
        }
        else if (_currentStory.currentChoices.Count > 0)
        {
            _currentState = DialogueState.PlayerChoice;
        }
        else if (!_currentStory.canContinue)
        {
            Debug.Log("Can't continue");
            EndDialogue();
            return;
        }
        else if(_currentState != DialogueState.PlayerSuggest)
        {
            _currentState = DialogueState.SimplePhrase;
        }

        InvokeStateEvent();
    }

    private void InvokeStateEvent()
    {
        switch (_currentState)
        {
            case DialogueState.SimplePhrase:
                ShowNewPhrase();
                break;
            case DialogueState.PlayerChoice:
                ParsePlayerChoices();
                break;
            case DialogueState.TagState:
                ParseTags();
                break;
            case DialogueState.PlayerSuggest:
                LetPlayerChooseDrink();
                break;
            case DialogueState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void LetPlayerChooseDrink()
    {
        _dialogueEventInvoker.Invoke(EventType.PlayerSuggest);
    }

    private void ParsePlayerChoices() => ParsePlayerChoices(_currentStory.currentChoices, MakeDecision);

    private void ParsePlayerChoices(List<Choice> choices, UnityAction<int> chooseAction)
    {
        _currentStory.currentChoices.ForEach(choice => 
            choice.text = InkyParser.ParsePhrase(choice.text, _currentDialogue.Participants).ColoredPhrase);
        _dialogueUI.ShowChoices(choices, chooseAction);
    }

    private void ParseTags()
    {
        var phraseTag = _currentStory.currentTags[0];
        if (Enum.TryParse(phraseTag, true, out EventType parsedTag))
            _dialogueEventInvoker.Invoke(parsedTag);
    }

    private void TryParseErrors()
    {
        if (_dialogueUI.IsTypingPhrase)
            throw new UnityException("Trying to update dialogue while typing phrase");
        if (CurrentDialogue == null)
            throw new UnityException("No dialogue");
        if (_currentStory == null)
            throw new UnityException("No story");
    }

    private void ShowNewPhrase(Drink overrideDrink = null)
    {
        var text = _currentStory.Continue();
        if (text == string.Empty)
        {
            UpdateDialogueState();
            return;
        }
        var data = InkyParser.ParsePhrase(text, CurrentDialogue.Participants, overrideDrink);

        if (data.Drink != null && overrideDrink == null && data.Drink.KeyName == DatabaseManager.DrinkDatabase.AnythingKeyName)
        {
            _nextState = DialogueState.PlayerSuggest;
        }

        ParseOrderData(data);

        _dialogueUI.SetCharacterUI(
            data.PhraseCharacter.Portrait,
            data.PhraseCharacter.CharacterName);

        _dialogueUI.ShowSimplePhrase(data.SimplePhrase, data.ColoredPhrase);
    }

    private void ParseOrderData(InkyParser.PhraseData data)
    {
        if (data.Drink != null 
            && data.Drink.KeyName != DatabaseManager.DrinkDatabase.AnythingKeyName 
            && CurrentDialogue.CurrentOrderData.SuggestedDrink == null)
        {
            CurrentDialogue.CurrentOrderData.Character = data.PhraseCharacter;
            CurrentDialogue.CurrentOrderData.Drink = data.Drink;
        }

        if(CurrentDialogue.CurrentOrderData.Character == null)
            CurrentDialogue.CurrentOrderData.Character = data.PhraseCharacter;
    }

    private void MakeDecision(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        //_currentStory.Continue();
        UpdateDialogueState();
    }

    private IEnumerator WaitForDrink()
    {
        yield return new WaitUntil(() => !OrderCreationEvents.Instance.DrinkInWork);
        CurrentDialogue.CurrentOrderData.Grade =
            CurrentDialogue.CurrentOrderData.Character
                .GetCharacterGrade(
                    CurrentDialogue.CurrentOrderData.Drink, 
                    OrderCreationEvents.Instance.OrderActionsTracker.GetOrderActions()
                    );
        PlayerStorage.MoneyData.AddOrderMoney(
            CurrentDialogue.CurrentOrderData.Drink,
            CurrentDialogue.CurrentOrderData.Grade);
        CurrentDialogue.CurrentOrderData.Character = null;
        CurrentDialogue.CurrentOrderData.Drink = null;
        UpdateDialogueState(true);
    }

    private void CreateTagEvents()
    {
        _dialogueEvents[EventType.Order].AddListener(() =>
        {
            GameStateManager.SwitchToWorkSpace();
            OrderCreationEvents.Instance.StartCreatingDrink(CurrentDialogue.CurrentOrderData.Drink);
            StartCoroutine(WaitForDrink());
        });

        _dialogueEvents[EventType.Check].AddListener(() =>
        {
            //TODO: implement check later
            var check = CurrentDialogue.CurrentOrderData.Drink != null 
                        && !CurrentDialogue.CurrentOrderData.Drink.InfoData.Locked;
            if (check)
            {
                _currentState = DialogueState.PlayerChoice;
                ParsePlayerChoices(
                    _currentStory.currentChoices.Take(2).ToList(),
                    index =>
                    {
                        _currentStory.ChooseChoiceIndex(index);
                        if (index == 1)
                        {
                            _dialogueEventInvoker.Invoke(EventType.PlayerSuggest);
                            return;
                        }
                        //_currentStory.Continue();
                        UpdateDialogueState(true);
                    });
            }
            else
            {
                _currentStory.ChooseChoiceIndex(2);
                UpdateDialogueState(true);
            }
        });

        _dialogueEvents[EventType.Grade].AddListener(() =>
        {
            _currentStory.ChooseChoiceIndex((int)CurrentDialogue.CurrentOrderData.Grade);
            UpdateDialogueState(true);
        });

        _dialogueEvents[EventType.GuestChoice].AddListener(() =>
        {
            var check = CurrentDialogue.CurrentOrderData.Character
                .CheckSuggestedDrink(CurrentDialogue.CurrentOrderData.SuggestedDrink);
            if (check)
            {
                CurrentDialogue.CurrentOrderData.Drink = CurrentDialogue.CurrentOrderData.SuggestedDrink;
                _currentStory.ChooseChoiceIndex(0);
            }
            else
            {
                _currentStory.ChooseChoiceIndex(1);
            }

            CurrentDialogue.CurrentOrderData.SuggestedDrink = null;
            UpdateDialogueState(true);
        });

        _dialogueEvents[EventType.Wait].AddListener(() =>
        {
            Debug.Log("Not implemented WAIT");
        });
    }
}