using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    private TMP_Text _textLabel;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _textLabel = _button.targetGraphic.GetComponent<TMP_Text>();
    }

    public void SetChoice(int index, string text, Action<int> selectAction)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => selectAction(index));
        _textLabel.text = $"{index + 1}. {InkyParser.ParsePhrase(text).coloredPhrase}";
    }
}
