using System;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(TypeWriter))]
public class DialogueUI : MonoBehaviour
{
    #region DialogueText
    [SerializeField] private TMP_Text _dialogueTextLabel;
    [SerializeField] private DialogueChoice[] _dialogueChoices;
    private TypeWriter _typeWriter;
    #endregion
    #region CharactersUI
    [SerializeField] private Image _npcPortrait;
    [SerializeField] private Image _mcPortrait;
    [SerializeField] private TMP_Text _characterNameLabel;
    #endregion

    private void Awake()
    {
        _typeWriter = GetComponent<TypeWriter>();
    }

    #region TypeWriter
    public bool IsTypingPhrase => !_typeWriter.IsTypingEnded;

    public void ShowSimplePhrase(string simplePhrase, string coloredPhrase)
    {
        _typeWriter.StartTypingPhrase(simplePhrase, _dialogueTextLabel, coloredPhrase);
    }

    public void EndTyping() => _typeWriter.EndTyping();

    #endregion

    #region Choices

    public void ShowChoices(List<Choice> choices, UnityAction<int> chooseAction)
    {
        _dialogueTextLabel.enabled = false;
        for (var i = 0; i < choices.Count; i++)
        {
            _dialogueChoices[i].gameObject.SetActive(true);
            _dialogueChoices[i].SetChoice(
                i,
                //InkyParser.ParsePhrase(choices[i].text).ColoredPhrase,
                choices[i].text,
                index =>
                {
                    chooseAction.Invoke(index);
                    _dialogueChoices.ForEachAction(choice => choice.gameObject.SetActive(false));
                    _dialogueTextLabel.enabled = true;
                });
        }
    }

    #endregion

    #region CharactersUI

    public void SetCharacterUI(Sprite portrait, CharacterType characterType, string characterName)
    {
        _characterNameLabel.text = characterName;
        _npcPortrait.Fade(0);
        _mcPortrait.Fade(0);
        switch (characterType)
        {
            case CharacterType.MainCharacter:
                _mcPortrait.sprite = portrait;
                _mcPortrait.Fade(1);
                break;
            case CharacterType.NPC:
                _npcPortrait.sprite = portrait;
                _npcPortrait.Fade(1);
                break;
            case CharacterType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(characterType), characterType, null);
        }
    }

    #endregion
}
