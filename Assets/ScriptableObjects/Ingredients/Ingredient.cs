using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Ingredient : ScriptableObject, IDatabaseObject, ISpriteDatabaseObject
{
    [SerializeField] private string _keyName;
    [SerializeField] private IngredientInfoData _infoData;

    protected abstract IngredientTypeData.IngredientType Type { get; }

    public IngredientInfoData Data => _infoData;

    public string KeyName => _keyName;

    public void WriteData(string[] paramsLine)
    {
        if (paramsLine.Length != 4)
        {
            paramsLine.ForEachAction(Debug.Log);
            throw new UnityException("Wrong params line");
        }
        _infoData.ParseData(paramsLine[1],
            int.Parse(paramsLine[2]),
            float.Parse(paramsLine[3]));
    }

    public abstract bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type);

    public abstract GameObject SpawnWorkItem(Transform container);
    public bool TakeMousePosition() => Type == IngredientTypeData.IngredientType.Drop;

    public Sprite Sprite
    {
        get => _infoData.ObjectSprite;
        set => _infoData.ObjectSprite = value;
    }


    [System.Serializable]
    public class IngredientInfoData : InfoData
    {
        public enum ClassType
        {
            Alcohol,
            Ingredient
        }

        public enum CountType
        {
            Liters,
            Grams,
            Pieces
        }

        [SerializeField] private CountType _countType;
        [SerializeField] private float _buyQuantityStep;
        [SerializeField] private int _costPerObject;
        [SerializeField] private bool _ignorePurchase;
        [SerializeField] private ClassType _class;
        [SerializeField] private List<Drink> _containingDrinks;

        public float BuyQuantityStep => _buyQuantityStep;

        public int CostPerObject => _costPerObject;

        public ClassType Class => _class;

        public List<Drink> ContainingDrinks
        {
            get => _containingDrinks;
            set => _containingDrinks = value;
        }

        public CountType TypeOfCount => _countType;

        public bool IgnorePurchase => _ignorePurchase;

        public void ParseData(string name, int costPerObject, float buyQuantityStep)
        {
            _name = name;
            _costPerObject = costPerObject;
            _buyQuantityStep = buyQuantityStep;
        }

        public string GetPurchaseSuffix()
        {
            return TypeOfCount switch
            {
                CountType.Liters => "כ",
                CountType.Grams => "דנ",
                CountType.Pieces => "רע",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}