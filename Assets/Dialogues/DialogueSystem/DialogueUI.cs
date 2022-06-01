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
    [SerializeField] private Image _portrait;
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
        for (var i = 0; i < choices.Count; i++)
        {
            _dialogueChoices[i].gameObject.SetActive(true);
            _dialogueChoices[i].SetChoice(
                i,
                choices[i].text,
                index =>
                {
                    chooseAction.Invoke(index);
                    _dialogueChoices.ForEachAction(choice => choice.gameObject.SetActive(false));
                });
        }
    }

    #endregion

    #region CharactersUI

    public void SetCharacterUI(Sprite portrait, string characterName)
    {
        _characterNameLabel.text = characterName;
        _portrait.sprite = portrait;
    }

    #endregion
}
