using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Data/Database/CharacterDatabase")]
public class CharacterDatabase : Database<Character>
{
    [SerializeField] private List<CharacterNameData> _storyCharactersNameDatas;
    [SerializeField] private List<CharacterNameData> _randomCharactersNameDatas;
    private Dictionary<string, string> _storyCharactersDictionary;
    private Dictionary<string, string> _randomCharactersDictionary;

    [Space(10f)]
    [SerializeField] private TextAsset _nameTable;
    [SerializeField] private TextAsset _coefficientsTable;

    public void LoadNameTableData()
    {
        _storyCharactersDictionary.Clear();
        var data = TablesParser.GetParsedTable(_nameTable);
        data.ForEachAction(names =>
            _storyCharactersNameDatas.Add(new CharacterNameData(names[0], names[1])));
    }

    public string TryGetName(string keyName)
    {
        if(_storyCharactersDictionary == null || _randomCharactersDictionary == null)
            FillDictionaries();
        if (_storyCharactersDictionary.ContainsKey(keyName))
            return _storyCharactersDictionary[keyName];
        if (_randomCharactersDictionary.TryGetObject(
                keyName, 
                (pair, s) => s.Contains(pair.Key), 
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
        _storyCharactersNameDatas.ForEachAction(data => _storyCharactersDictionary.Add(data.KeyName, data.Name));
        _randomCharactersNameDatas.ForEachAction(data => _randomCharactersDictionary.Add(data.KeyName, data.Name));

        Debug.Log($"Successfully filled: \nStory characters dictionary - {_storyCharactersDictionary.Count}\nRandom characters dictionary - {_randomCharactersDictionary.Count}");
    }

    public void ClearDictionaries()
    {
        _storyCharactersDictionary.Clear();
        _randomCharactersDictionary.Clear();
        Debug.Log($"Successfully cleared: \nStory characters dictionary - {_storyCharactersDictionary.Count}\nRandom characters dictionary - {_randomCharactersDictionary.Count}");
    }


    [System.Serializable]
    private struct CharacterNameData
    {
        public string KeyName;
        public string Name;

        public CharacterNameData(string keyName, string name)
        {
            KeyName = keyName;
            Name = name;
        }
    }
}