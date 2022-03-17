using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueSystem))]
public class DialogueEvents : MonoBehaviour
{
    public enum EventType
    {
        End,
        Order,
        Wait,
        GuestChoice,
        Check,
        Grade,
        DrinkSuggestedByGuest
    }

    [SerializeField] private UnityEvent<Guest> _dialogueEndEvent;

    private DialogueSystem _dialogueSystem;
    private readonly FastAction<EventType> _dialogueEventInvoker = new FastAction<EventType>();

    private readonly Dictionary<EventType, FastAction> _dialogueEvents = 
        new Dictionary<EventType, FastAction>
        {
            [EventType.End] = new FastAction(),
            [EventType.Order] = new FastAction(),
            [EventType.Wait] = new FastAction(),
            [EventType.GuestChoice] = new FastAction(),
            [EventType.Check] = new FastAction(),
            [EventType.Grade] = new FastAction(),
            [EventType.DrinkSuggestedByGuest] = new FastAction()
        };

    private void Awake()
    {
        _dialogueSystem = GetComponent<DialogueSystem>();
        _dialogueEventInvoker.Add(type => _dialogueEvents[type].Call());
        _dialogueEvents[EventType.Check].Add(() => _dialogueSystem.MakeTagDecision(0));
        _dialogueEvents[EventType.Order].Add(() =>
        {
            GamePartsSwitch.StartCreatingOrder(_dialogueSystem.ChosenDrink);
        });
    }

    public void StartDialogue(Guest guest)
    {
        _dialogueEvents[EventType.End].Add(() => _dialogueEndEvent.Invoke(guest));

        _dialogueEvents[EventType.GuestChoice].Add(() => _dialogueSystem.MakeTagDecision(guest.GetGuestChoice() ? 0 : 1));

        _dialogueEvents[EventType.Grade].Add(() => _dialogueSystem.MakeTagDecision((int) guest.GetCharacterGrade()));

        _dialogueSystem.StartDialogue(guest.CurrentDialogue, _dialogueEventInvoker);
    }

    public void ContinueDialogue()
    {
        _dialogueSystem.ContinueDialogue();
    }
}
