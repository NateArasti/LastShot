using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleCharacter", menuName = "Character")]
public class Character : ScriptableObject, IDatabaseObject
{
    [SerializeField] private string _keyName;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private CharacterType _characterType = CharacterType.None;
    [SerializeField] private TextAsset _dialogue;
    [SerializeField] private TextAsset _coefficientsTable;
    [SerializeField] private Drink _drink;

    private bool _nameWritten;
    private Dictionary<string, float> _coefficientsDictionary;

    public string CharacterName { get; private set; }
    public string KeyName => _keyName;

    public Drink Drink => _drink;
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

    public Sprite Portrait => _portrait;
    public CharacterType CharacterType => _characterType;
    public TextAsset Dialogue => _dialogue;

    private void WriteName(string nameToWrite)
    {
        if(_nameWritten) return;
        CharacterName = nameToWrite;
        _nameWritten = true;
    }
}
