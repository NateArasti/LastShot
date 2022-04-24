using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleCharacter", menuName = "Characters/Character")]
public class Character : ScriptableObject, IDatabaseObject
{
    [SerializeField] private string _keyName;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private CharacterType _characterType = CharacterType.None;
    [SerializeField] private TextAsset _coefficientsTable;

    private bool _nameWritten;
    private Dictionary<string, float> _coefficientsDictionary;

    public string CharacterName { get; set; }
    public Sprite Portrait => _portrait;
    public CharacterType CharacterType => _characterType;
    public string KeyName => _keyName;
    public GameObject Prefab => _prefab;

    public float GetCoefficient(string drinkKeyName)
    {
        if (_coefficientsDictionary.ContainsKey(drinkKeyName))
            return _coefficientsDictionary[drinkKeyName];
        throw new UnityException($"{drinkKeyName} no such drink in {name} character coefficients table");
    }

    public bool GetGuestChoice() => true;

    public CharacterGuestGrade GetCharacterGrade()
    {
        return CharacterGuestGrade.Excellent;
    }

    public void WriteData(string[] paramsLine)
    {
        WriteName(paramsLine[1]);
        if(_coefficientsTable != null)
            _coefficientsDictionary = TablesParser.GetParsedCoefficientsTable(_coefficientsTable);
    }

    private void WriteName(string nameToWrite)
    {
        if(_nameWritten) return;
        CharacterName = nameToWrite;
        _nameWritten = true;
    }
}
