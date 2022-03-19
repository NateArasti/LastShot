using UnityEngine;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable IdentifierTypo

[CreateAssetMenu(fileName = "Simple Drink", menuName = "Drink")]
public class Drink : ScriptableObject, IDatabaseObject
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
    [Header("Info Data")]
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Tag[] _tags;
    [TextArea(5, 20)] [SerializeField] private string _description;
    [TextArea(5, 20)] [SerializeField] private string _textReceipt;

    [Header("Receipt")]
    [SerializeField] private GameObject _glassPrefab;
    [SerializeField] private GameObject[] _instruments;
    [SerializeField] private Ingredient[] _alcohols;
    [SerializeField] private Ingredient[] _additionalIngredients;
    [SerializeField] private OrderAction[] _perfectActions;


    public Sprite Sprite
    {
        get => _icon;
        set => _icon = value;
    }

    public (
        IReadOnlyList<OrderAction> perfectActions,
        GameObject glassPrefab,
        IReadOnlyCollection<Ingredient> alcohols,
        IReadOnlyCollection<Ingredient> ingredients,
        IReadOnlyCollection<IWorkItem> instruments
        ) Receipt
    {
        get
        {
            var instruments = _instruments
                .Select(instrument => instrument.GetComponent<IWorkItem>())
                .ToList();
            return (_perfectActions, _glassPrefab, _alcohols, _additionalIngredients, instruments);
        }
    }

    public (
        string name,
        Tag[] tags,
        string description,
        string textReceipt,
        Sprite icon
        ) InfoData => (_name, _tags, _description, _textReceipt, _icon);

    public string KeyName => _keyName;

    public void WriteData(string[] paramsLine)
    {
        _name = paramsLine[1];
    }
}