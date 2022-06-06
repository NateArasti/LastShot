using UnityEngine;
using System;

[Serializable]
public class OrderAction
{
    [HideInInspector, SerializeField] private string _actionName;
    [SerializeField] private float _weight;

    public float Weight => _weight;

    /// <summary>
    /// Compares actions that were made to drink reference actions and returns value in range (0, 1) representing comparison
    /// </summary>
    /// <param name="referenceActions"></param>
    /// <param name="orderActions"></param>
    /// <returns></returns>
    public static float Compare(OrderAction[] referenceActions, OrderAction[] orderActions)
    {
        return 1;//TODO: implement correct comparison
    }

    public void SetActionName() => _actionName = ToString();

    public override string ToString()
    {
        return GetType().Name;
    }

    //Derives

    [Serializable]
    public class IngredientAddAction : OrderAction
    {
        [SerializeField] private Ingredient _ingredient;
        [SerializeField] private float _quantity;

        public Ingredient Ingredient => _ingredient;
        public float Quantity => _quantity;
    }

    [Serializable]
    public class IngredientAddToShakerAction : IngredientAddAction
    {
    }

    [Serializable]
    public class DecorateAction : IngredientAddAction
    {
    }

    [Serializable]
    public class SpoonMixAction : OrderAction
    {
        [SerializeField, Range(0, 1)] private float _intensity;
        public float Intensity => _intensity;
    }

    [Serializable]
    public class SpoonLayerAction : OrderAction
    {
        [SerializeField] private Ingredient _ingredient;
        [SerializeField] private float _quantity;

        public Ingredient Ingredient => _ingredient;
        public float Quantity => _quantity;
    }

    [Serializable]
    public class ShakerMixAction : OrderAction
    {
        [SerializeField, Range(0, 1)] private float _intensity;
        public float Intensity => _intensity;
    }
    [Serializable]
    public class ShakerPourAction : OrderAction
    {
    }

    [Serializable]
    public class FireAction : OrderAction
    {
        [SerializeField] private float _fireTime;
        public float FireTime => _fireTime;
    }
}