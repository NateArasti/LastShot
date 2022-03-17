using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactersUI : MonoBehaviour
{
    [SerializeField] private Image _npcPortrait;
    [SerializeField] private Image _mcPortrait;
    [SerializeField] private TMP_Text _characterNameLabel;

    private Image _previousPortrait;

    private void Awake()
    {
        _previousPortrait = _npcPortrait;
    }

    public void ShowCharacter(Character character)
    {
        _characterNameLabel.text = character.CharacterName;
        if (character.CharacterType == CharacterType.None)
        {
            SwitchColorAlpha(_previousPortrait, 0);
            return;
        }

        SwitchCharacterPortrait(
            character.CharacterType == CharacterType.MainCharacter ? _mcPortrait : _npcPortrait,
            character.Portrait
            );
    }

    private void SwitchCharacterPortrait(Image portraitLabel, Sprite newPortrait)
    {
        SwitchColorAlpha(_previousPortrait, 0);
        SwitchColorAlpha(portraitLabel, 1);
        portraitLabel.sprite = newPortrait;
        _previousPortrait = portraitLabel;
    }

    private void SwitchColorAlpha(Image portraitLabel, int newAlpha)
    {
        var color = portraitLabel.color;
        color.a = newAlpha;
        portraitLabel.color = color;
    }
}