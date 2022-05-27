using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public  abstract class Database<T> : ScriptableObject where T : Object, IDatabaseObject, ISpriteDatabaseObject
{
    [SerializeField] private string _resourcesDatabase;
    [SerializeField] private T[] _databaseObjects;
    [SerializeField] private TextAsset _dataTable;
    [SerializeField] private string _spritesFolderPath;

    private readonly Dictionary<string, T> _dataDictionary = new();

    public void LoadFromResources()
    {
        _databaseObjects = Resources.LoadAll<T>(_resourcesDatabase);
        OnValidate();
        Debug.Log($"Loaded Objects for {name} successfully");
    }

    public void LoadSpritesFromResources()
    {
        foreach (var sprite in Resources.LoadAll<Sprite>(_spritesFolderPath))
        {
            if (TryGetValue(sprite.name, out var obj))
            {
                obj.Sprite = sprite;
                EditorUtility.SetDirty(obj);
            }
            else
            {
                Debug.LogWarning($"No object with {sprite.name}");
            }
        }
        Debug.Log($"Loaded Sprites for {name} successfully");
    }

    public void LoadTableData()
    {
        if (_dataTable == null)
        {
            Debug.LogError("No table to load!!!");
            return;
        }
        this.WriteParsedTableData(_dataTable);
        Debug.Log($"Loaded Table Data for {name} successfully");
    }

    public bool TryGetValue(string keyName, out T value)
    {
        if (_dataDictionary.ContainsKey(keyName))
        {
            value = _dataDictionary[keyName];
            return true;
        }

        value = default;
        return false;
    }

    public IReadOnlyCollection<T> GetObjectsCollection() => _databaseObjects;

    private void OnValidate()
    {
        foreach (var obj in _databaseObjects)
        {
            if(obj != null)
                _dataDictionary[obj.KeyName] = obj;
        }
    }

    public void Distinct()
    {
        var newList = new List<T>();
        var storageItemSet = new HashSet<string>();
        foreach (var storageItem in _databaseObjects)
        {
            if (storageItemSet.Contains(storageItem.KeyName)) continue;
            storageItemSet.Add(storageItem.KeyName);
            newList.Add(storageItem);
        }

        _databaseObjects = newList.ToArray();
        OnValidate();
    }
}
