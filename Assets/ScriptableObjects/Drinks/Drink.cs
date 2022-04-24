using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

// ReSharper disable IdentifierTypo

[CreateAssetMenu(fileName = "Simple Drink", menuName = "Drink")]
public class Drink : ScriptableObject, IDatabaseObject, ISpriteDatabaseObject
{
    public enum Tag
    {
        Крепкий,
        Слабоалкогольный,
        Цитрусовый,
        Сладкий,
        Горький,
        Ягодный,
        Кислый,
        Солёный,
        Освежающий,
        Острый,
        Сливочный,
        Мятный,
        Простой
    }

    [SerializeField] private string _keyName;
    [Header("Info Data")] [SerializeField] private DrinkInfoData _infoData;
    [Header("Receipt")] [SerializeField] private Receipt _drinkReceipt;

    public Sprite Sprite
    {
        get => _infoData.ObjectSprite;
        set
        {
            if (EditorApplication.isPlaying) return;
            _infoData.ObjectSprite = value;
        }
    }

    public Receipt DrinkReceipt => _drinkReceipt;

    public DrinkInfoData InfoData => _infoData;

    public string KeyName => _keyName;

    public void WriteData(string[] paramsLine)
    {
        _infoData.ParseData(paramsLine[1]);
    }

    [System.Serializable]
    public struct Receipt
    {
        [SerializeField] private GameObject _glassPrefab;
        [SerializeField] private List<Ingredient> _ingredients;
        [SerializeField] private OrderAction[] _perfectActions;
        public IReadOnlyList<OrderAction> PerfectActions => _perfectActions;
        public GameObject GlassPrefab => _glassPrefab;
        public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;
    }

    [System.Serializable]
    public class DrinkInfoData : InfoData
    {
        [SerializeField] private Tag[] _tags;
        [TextArea(5, 20)] [SerializeField] private string _textReceipt;

        public Tag[] Tags => _tags;

        public string TextReceipt => _textReceipt;

        public void ParseData(string name)
        {
            _name = name;
        }
    }
}