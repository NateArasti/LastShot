using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

// ReSharper disable IdentifierTypo

[CreateAssetMenu(fileName = "Simple Drink", menuName = "Data/DataItem/Drink")]
public class Drink : ScriptableObject, IDatabaseObject, ISpriteDatabaseObject
{
    public enum Tag
    {
        Крепкий,
        Слабоалкогольный,
        Цитрусовый,
        Сладкий,
        Кислый,
        Горький,
        Пикантный
    }

    [SerializeField] private string _keyName;
    [Space(10f)]
    [SerializeField] private DrinkInfoData _infoData;
    [Space(10f)]
    [SerializeField] private Receipt _receipt;

    public Sprite Sprite
    {
        get => _infoData.ObjectSprite;
        set => _infoData.ObjectSprite = value;
    }

    public Receipt DrinkReceipt => _receipt;

    public DrinkInfoData InfoData => _infoData;

    public string KeyName => _keyName;

    public void WriteData(string[] paramsLine)
    {
        _infoData.ParseData(paramsLine[1]);
    }

    private void OnValidate()
    {
        foreach (var action in _receipt.PerfectActions)
        {
            action.SetActionName();
        }
    }

    [System.Serializable]
    public class DrinkInfoData : InfoData
    {
        [SerializeField] private Tag[] _tags;
        [TextArea(5, 20)] [SerializeField] private string _textReceipt;
        [SerializeField] private bool _locked;
        [SerializeField, HideInInspector] private float _cost;

        public float Cost => _cost;

        public Tag[] Tags => _tags;

        public string TextReceipt => _textReceipt;

        public bool Locked => _locked;

        public void ParseData(string name)
        {
            _name = name;
        }

        public void SetCost(float cost)
        {
            _cost = cost;
        }
    }

    [Serializable]
    public class Receipt
    {
        [SerializeField] private GameObject _glassPrefab;
        [SerializeField] private List<Instrument> _instruments;
        [SerializeField] private List<Ingredient> _ingredients;
        [SerializeField, SerializeReference] private List<OrderAction> _perfectActions = new();

        public IReadOnlyList<OrderAction> PerfectActions => _perfectActions;
        public GameObject GlassPrefab => _glassPrefab;
        public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;
        public IReadOnlyCollection<Instrument> Instruments => _instruments;

        public void AddAction(Type actionType)
        {
            var action = new OrderAction();
            switch (actionType.Name)
            {
                case "IngredientAddAction":
                    action = new OrderAction.IngredientAddAction();
                    break;
                case "IngredientAddToShakerAction":
                    action = new OrderAction.IngredientAddToShakerAction();
                    break;
                case "DecorateAction":
                    action = new OrderAction.DecorateAction();
                    break;
                case "SpoonMixAction":
                    action = new OrderAction.SpoonMixAction();
                    break;
                case "ShakerMixAction":
                    action = new OrderAction.ShakerMixAction();
                    break;
                case "ShakerPourAction":
                    action = new OrderAction.ShakerPourAction();
                    break;
                case "SpoonLayerAction":
                    action = new OrderAction.SpoonLayerAction();
                    break;
                case "FireAction":
                    action = new OrderAction.FireAction();
                    break;
                default:
                    Debug.LogError($"Not implemented type {actionType.Name}");
                    break;
            }

            _perfectActions = new List<OrderAction>(_perfectActions)
            {
                action
            };
        }

        public void RemoveAction()
        {
            _perfectActions.RemoveAt(_perfectActions.Count - 1);
        }

        public float CountPrimeCost()
        {
            var count = 0f;
            foreach (var action in _perfectActions)
            {
                if (action is OrderAction.IngredientAddAction addAction && 
                    addAction.Ingredient != null &&
                    addAction.Ingredient.Data.BuyQuantityStep != 0)
                {
                    count += 
                        addAction.Quantity *
                        addAction.Ingredient.Data.CostPerObject / addAction.Ingredient.Data.BuyQuantityStep;
                }
            }

            return count;
        }
    }
}