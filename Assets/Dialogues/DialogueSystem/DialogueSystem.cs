using System;
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

    [SerializeField] private Dialogue _dialogue;

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

    private Dialogue _currentDialogue;

    private Dialogue.OrderData _currentOrderData;
    private DialogueState _currentState;
    private Story _currentStory;
    private DialogueUI _dialogueUI;

    private void Awake()
    {
        _dialogueUI = GetComponent<DialogueUI>();
        _dialogueEventInvoker.AddListener(type => _dialogueEvents[type].Invoke());

        _dialogueEvents[EventType.PlayerSuggest].AddListener(() =>
        {
            _currentOrderData.Drink = DatabaseManager.DrinkDatabase.GetObjectsCollection().ToList().GetRandomObject();
            print(_currentOrderData.Drink);
            print(_currentOrderData.Character.KeyName);
        });
        CreateTagEvents();
    }

    private void Start()
    {
        StartDialogue(_dialogue);
    }

    private void Update()
    {
        if (_currentState != DialogueState.SimplePhrase || !KeyCodesToSkip.Any(Input.GetKeyDown)) return;
        if (_dialogueUI.IsTypingPhrase)
            _dialogueUI.EndTyping();
        else
            UpdateDialogueState();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (_currentState != DialogueState.None) throw new UnityException("Dialogue is already going!!!");
        _currentDialogue = dialogue;
        _currentStory = new Story(_currentDialogue.Text.text);
        UpdateDialogueState();
    }

    private void EndDialogue()
    {
        _currentDialogue = null;
        _currentStory = null;
        _currentState = DialogueState.None;
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

    private void ParsePlayerChoices()
    {
        _dialogueUI.ShowChoices(_currentStory.currentChoices, MakeDecision);
    }

    private void ParsePlayerChoices(List<Choice> choices, UnityAction<int> chooseAction)
    {
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
        if (_currentDialogue == null)
            throw new UnityException("No dialogue");
        if (_currentStory == null)
            throw new UnityException("No story");
    }

    private void ShowNewPhrase()
    {
        var text = _currentStory.Continue();
        if (text == string.Empty)
        {
            UpdateDialogueState();
            return;
        }
        var data = InkyParser.ParsePhrase(text, _currentDialogue.Participants);

        ParseOrderData(data);

        _dialogueUI.SetCharacterUI(
            data.PhraseCharacter.Portrait,
            data.PhraseCharacter.CharacterType,
            data.PhraseCharacter.CharacterName);

        _dialogueUI.ShowSimplePhrase(data.SimplePhrase, data.ColoredPhrase);
    }

    private void ParseOrderData(InkyParser.PhraseData data)
    {

        if (data.Drink != null)
        {
            if (data.Drink.KeyName == "ANYTHING")
            {
                _currentState = DialogueState.PlayerSuggest;
                return;
            }
            _currentOrderData.Drink = data.Drink;
            _currentOrderData.Character = null;
            if (data.PhraseCharacter.CharacterType != CharacterType.MainCharacter)
                _currentOrderData.Character = data.PhraseCharacter;
        }

        if(_currentOrderData.Character == null)
            _currentOrderData.Character = data.PhraseCharacter;
    }

    private void MakeDecision(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        //_currentStory.Continue();
        UpdateDialogueState();
    }

    private void CreateTagEvents()
    {
        _dialogueEvents[EventType.Order].AddListener(() =>
        {
            _currentOrderData.Grade = _currentOrderData.Character.GetCharacterGrade();
            //_currentOrderData.Grade = CharacterGuestGrade.Excellent;
            UpdateDialogueState(true);
        });

        _dialogueEvents[EventType.Check].AddListener(() =>
        {
            var check = true;
            if (check)
            {
                _currentState = DialogueState.PlayerChoice;
                ParsePlayerChoices(
                    _currentStory.currentChoices.Take(2).ToList(),
                    index =>
                    {
                        _currentStory.ChooseChoiceIndex(index);
                        _currentStory.Continue();
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
            _currentStory.ChooseChoiceIndex((int) _currentOrderData.Grade);
            UpdateDialogueState(true);
        });

        _dialogueEvents[EventType.GuestChoice].AddListener(() =>
        {
            _currentStory.ChooseChoiceIndex(0);
            UpdateDialogueState(true);
        });

        _dialogueEvents[EventType.Wait].AddListener(() =>
        {

        });
    }
}