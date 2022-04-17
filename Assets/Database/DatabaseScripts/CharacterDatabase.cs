using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Data/Database/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] private List<CharacterNameData> _storyCharactersNameDatas;
    [SerializeField] private List<CharacterNameData> _randomCharactersNameDatas;
    private Dictionary<string, string> _storyCharactersDictionary;
    private Dictionary<string, string> _randomCharactersDictionary;

    public string TryGetName(string keyName)
    {
        if(_storyCharactersDictionary == null || _randomCharactersDictionary == null)
            FillDictionaries();
        if (_storyCharactersDictionary.ContainsKey(keyName))
            return _storyCharactersDictionary[keyName];
        if (_randomCharactersDictionary.TryGetObject(
                keyName, 
                (pair, s) =>
                {
                    return s.Contains(pair.Key);
                }, 
                out var characterPair))
        {
            return characterPair.Value;
        }

        return default;
    }

    public void FillDictionaries()
    {
        _storyCharactersDictionary = new Dictionary<string, string>();
        _randomCharactersDictionary = new Dictionary<string, string>();
        _storyCharactersNameDatas.ForEachAction(data => _storyCharactersDictionary.Add(data._keyName, data._name));
        _randomCharactersNameDatas.ForEachAction(data => _randomCharactersDictionary.Add(data._keyName, data._name));
        Debug.Log($"Successfully filled: \nStory characters dictionary - {_storyCharactersDictionary.Count}\nRandom characters dictionary - {_randomCharactersDictionary.Count}");
    }

    public void ClearDictionaries()
    {
        _storyCharactersDictionary.Clear();
        _randomCharactersDictionary.Clear();
        Debug.Log($"Successfully cleared: \nStory characters dictionary - {_storyCharactersDictionary.Count}\nRandom characters dictionary - {_randomCharactersDictionary.Count}");
    }

    [Space(10f)]
    [SerializeField] private TextAsset _table;
    public void LoadTableData()
    {
        var data = TablesParser.GetParsedTable(_table);
        data.ForEachAction(names =>
            _storyCharactersNameDatas.Add(new CharacterNameData(names[0], names[1])));
    }

    [System.Serializable]
    private struct CharacterNameData
    {
        public string _keyName;
        public string _name;

        public CharacterNameData(string keyName, string name)
        {
            _keyName = keyName;
            _name = name;
        }
    }
}