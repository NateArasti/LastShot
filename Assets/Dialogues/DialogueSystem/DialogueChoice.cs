using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(TMP_Text))]
public class DialogueChoice : MonoBehaviour
{
    private TMP_Text _textLabel;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _textLabel = _button.targetGraphic.GetComponent<TMP_Text>();
    }

    public void SetChoice(int index, string text, UnityAction<int> selectAction)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => selectAction(index));
        _textLabel.text = $"{index + 1}. {text}";
    }
}
