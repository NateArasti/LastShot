using System;
using System.Collections;
using System.Linq;
using Ink.Runtime;
using TMPro;
using UnityEngine;

// ReSharper disable MustUseReturnValue

[RequireComponent(typeof(TypeWriter))]
public class DialogueSystem : MonoBehaviour
{
    private static readonly KeyCode[] KeyCodesToSkip =
    {
        KeyCode.Space,
        KeyCode.E,
        KeyCode.Mouse0,
        KeyCode.Return
    };

    [SerializeField] private DialogueChoice[] _dialogueChoices;
    [SerializeField] private CharactersUI _charactersUI;
    [SerializeField] private TMP_Text _dialogueTextLabel;
    private Story _currentDialogue;
    private bool _decisionIsMade;
    private bool _dialogueIsGoing;

    private FastAction<DialogueEvents.EventType> _onDialogueEvent;

    private TypeWriter _typeWriter;

    public Drink ChosenDrink { get; private set; }

    private void Awake()
    {
        _typeWriter = GetComponent<TypeWriter>();
    }

    private void Update()
    {
        if (!_dialogueIsGoing) return;
        if (KeyCodesToSkip.Any(Input.GetKeyDown)) UpdateDialogue();
    }

    public void StartDialogue(TextAsset dialogue, FastAction<DialogueEvents.EventType> dialogueEventInvoker)
    {
        if (_dialogueIsGoing)
            throw new UnityException("Dialogue is already going!!!");

        _onDialogueEvent = dialogueEventInvoker;
        _currentDialogue = new Story(dialogue.text);
        _dialogueIsGoing = true;
        ShowNewPhrase();
    }

    private void EndDialogue()
    {
        _dialogueIsGoing = false;
        _onDialogueEvent.Call(DialogueEvents.EventType.End);
    }

    private void UpdateDialogue(bool ignoreTags = false)
    {
        if (_typeWriter.IsTypingEnded)
        {
            if (_currentDialogue.currentTags.Count > 0 && !ignoreTags)
            {
                var dialogueTag = _currentDialogue.currentTags[0];
                if (!Enum.TryParse<DialogueEvents.EventType>(dialogueTag, true, out var eventType))
                    throw new UnityException($"No such tag - {dialogueTag}");
                _onDialogueEvent.Call(eventType);
                return;
            }

            if (_currentDialogue.currentChoices.Count != 0)
                StartCoroutine(ShowChoices());
            else if (!_currentDialogue.canContinue)
                EndDialogue();
            else
                ShowNewPhrase();
        }
        else
        {
            _typeWriter.EndTyping();
        }
    }

    private void ShowNewPhrase()
    {
        var currentText = _currentDialogue.Continue();
        
        if (currentText == string.Empty || currentText == "event\n")
        {
            UpdateDialogue();
            return;
        }

        var (character, alcohol, coloredPhrase, phrase) = 
            InkyParser.ParsePhrase(currentText);
        _charactersUI.ShowCharacter(character);
        _typeWriter.StartTypingPhrase(phrase, _dialogueTextLabel, coloredPhrase);
        if(character.CharacterType == CharacterType.NPC)
            ChosenDrink = character.Drink;
        //if (alcohol != null && character.CharacterType == CharacterType.NPC)
        //    _onDialogueEvent.Call(DialogueEvents.EventType.DrinkSuggestedByGuest);
    }

    private IEnumerator ShowChoices()
    {
        _decisionIsMade = false;
        _dialogueTextLabel.enabled = false;
        var choices = _currentDialogue.currentChoices;
        for (var i = 0; i < choices.Count; i++)
        {
            _dialogueChoices[i].gameObject.SetActive(true);
            _dialogueChoices[i].SetChoice(choices[i].index, choices[i].text, MakeDecision);
        }

        yield return new WaitUntil(() => _decisionIsMade);
    }

    private void MakeDecision(int choiceIndex)
    {
        _currentDialogue.ChooseChoiceIndex(choiceIndex);
        UpdateDialogue();
        _decisionIsMade = true;
        _dialogueTextLabel.enabled = true;
        foreach (var choice in _dialogueChoices) choice.gameObject.SetActive(false);
    }

    public void MakeTagDecision(int choiceIndex)
    {
        _currentDialogue.ChooseChoiceIndex(choiceIndex);
        UpdateDialogue(ignoreTags: true);
    }

    public void ContinueDialogue()
    {
        UpdateDialogue(true);
    }
}