using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleCharacter", menuName = "Characters/Character")]
public class Character : ScriptableObject, IDatabaseObject
{
    [SerializeField] private string _keyName;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private Guest _prefab;

    private bool _nameWritten;
    private Dictionary<string, float> _coefficientsDictionary;

    public string CharacterName { get; set; }
    public Sprite Portrait => _portrait;
    public string KeyName => _keyName;
    public Guest Prefab => _prefab;

    public CharacterGuestGrade GetCharacterGrade()
    {
        return CharacterGuestGrade.Excellent;
    }

    public void WriteData(string[] paramsLine)
    {
        WriteName(paramsLine[1]);
    }

    private void WriteName(string nameToWrite)
    {
        if(_nameWritten) return;
        CharacterName = nameToWrite;
        _nameWritten = true;
    }
}
