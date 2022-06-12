using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class OrderAction
{
    [HideInInspector, SerializeField] private string _actionName;
    [SerializeField] private float _weight;
    public readonly bool Readonly;

    public float Weight => _weight;

    public OrderAction(bool readonlyCheck = true)
    {
        Readonly = readonlyCheck;
    }

    /// <summary>
    /// Compares actions that were made to drink reference actions and returns value in range (0, 1) representing comparison
    /// </summary>
    /// <param name="referenceActions"></param>
    /// <param name="orderActions"></param>
    /// <returns></returns>
    public static float Compare(IReadOnlyList<OrderAction> referenceActions, IReadOnlyList<OrderAction> orderActions)
    {
        //foreach (var orderAction in orderActions)
        //{
        //    Debug.Log(orderAction.ToString());
        //}
        //return 1;
        if (referenceActions == null) throw new ArgumentNullException(nameof(referenceActions));
        if (orderActions == null) throw new ArgumentNullException(nameof(orderActions));
        var n = referenceActions.Count;
        var m = orderActions.Count;
        var totalWeight = 0f;
        referenceActions.ForEachAction(action => totalWeight += action.Weight);

        var comparison = 0f;
        var referenceIndex = 0;
        for (var i = 0; i < m; i++)
        {
            for (var j = referenceIndex; j < n; ++j)
            {
                var compare = Compare(referenceActions[j], orderActions[i]);
                if (compare > 0)
                {
                    referenceIndex = j + 1;
                    var weight = referenceActions[j].Weight / totalWeight;
                    comparison += compare * weight;
                    Debug.LogError(compare);
                    break;
                }
            }
        }

        return comparison;
    }

    private static float Compare(OrderAction referenceAction, OrderAction orderAction)
    {
        if (referenceAction.GetType() != orderAction.GetType()) return 0;
        switch (referenceAction)
        {
            case IngredientAddToShakerAction castedReferenceAction:
            {
                var castedOrderAction = orderAction as IngredientAddAction;
                if (castedOrderAction.Ingredient.KeyName !=
                    castedReferenceAction.Ingredient.KeyName)
                    return 0;
                return 1 -
                       Mathf.Clamp01(Mathf.Abs(castedOrderAction.Quantity - castedReferenceAction.Quantity) /
                                     castedReferenceAction.Quantity);
            }
            case ShakerMixAction castedReferenceAction:
            {
                var castedOrderAction = orderAction as ShakerMixAction;
                return 1 -
                       Mathf.Clamp01(Mathf.Abs(castedOrderAction.Intensity - castedReferenceAction.Intensity) /
                                     castedReferenceAction.Intensity);
            }
            case ShakerPourAction castedReferenceAction:
            {
                return 1;
            }
            case SpoonMixAction castedReferenceAction:
            {
                var castedOrderAction = orderAction as SpoonMixAction;
                return 1 -
                       Mathf.Clamp01(Mathf.Abs(castedOrderAction.Intensity - castedReferenceAction.Intensity) /
                                     castedReferenceAction.Intensity);
                }
            case IngredientAddAction castedReferenceAction:
            {
                var castedOrderAction = orderAction as IngredientAddAction;
                if (castedOrderAction.Ingredient.KeyName !=
                    castedReferenceAction.Ingredient.KeyName)
                    return 0;
                return 1 -
                       Mathf.Clamp01(Mathf.Abs(castedOrderAction.Quantity - castedReferenceAction.Quantity) /
                                     castedReferenceAction.Quantity);
            }
            case DecorateAction castedReferenceAction:
            {
                var castedOrderAction = orderAction as DecorateAction;
                return castedOrderAction.Ingredient.KeyName !=
                       castedReferenceAction.Ingredient.KeyName ? 0 : 1;
            }
            default:
                throw new UnityException($"Not implemented action type - {referenceAction}");
        }
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

        public IngredientAddAction(bool readonlyCheck = true) : base(readonlyCheck) {}

        public Ingredient Ingredient
        {
            get => _ingredient;
            set
            {
                if(Readonly) return;
                _ingredient = value;
            }
        }

        public float Quantity
        {
            get => _quantity;
            set
            {
                if (Readonly) return;
                _quantity = value;
            }
        }
    }

    [Serializable]
    public class IngredientAddToShakerAction : IngredientAddAction
    {
        public IngredientAddToShakerAction(bool readonlyCheck = true) : base(readonlyCheck) { }
    }

    [Serializable]
    public class DecorateAction : OrderAction
    {
        [SerializeField] private Ingredient _ingredient;
        public Ingredient Ingredient
        {
            get => _ingredient;
            set
            {
                if (Readonly) return;
                _ingredient = value;
            }
        }
        public DecorateAction(bool readonlyCheck = true) : base(readonlyCheck) { }
    }

    [Serializable]
    public class SpoonMixAction : OrderAction
    {
        [SerializeField, Range(0, 1)] private float _intensity;
        public SpoonMixAction(bool readonlyCheck = true) : base(readonlyCheck) { }
        public float Intensity
        {
            get => _intensity;
            set
            {
                if (Readonly) return;
                _intensity = value;
            }
        }
    }

    [Serializable]
    public class SpoonLayerAction : OrderAction
    {
        [SerializeField] private Ingredient _ingredient;
        [SerializeField] private float _quantity;
        public SpoonLayerAction(bool readonlyCheck = true) : base(readonlyCheck) { }

        public Ingredient Ingredient
        {
            get => _ingredient;
            set
            {
                if (Readonly) return;
                _ingredient = value;
            }
        }

        public float Quantity
        {
            get => _quantity;
            set
            {
                if (Readonly) return;
                _quantity = value;
            }
        }
    }

    [Serializable]
    public class ShakerMixAction : OrderAction
    {
        [SerializeField, Range(0, 1)] private float _intensity;
        public ShakerMixAction(bool readonlyCheck = true) : base(readonlyCheck) { }
        public float Intensity
        {
            get => _intensity;
            set
            {
                if (Readonly) return;
                _intensity = value;
            }
        }
    }
    [Serializable]
    public class ShakerPourAction : OrderAction
    {
        public ShakerPourAction(bool readonlyCheck = true) : base(readonlyCheck) { }
    }

    [Serializable]
    public class FireAction : OrderAction
    {
        [SerializeField] private float _fireTime;
        public FireAction(bool readonlyCheck = true) : base(readonlyCheck) { }
        public float FireTime
        {
            get => _fireTime;
            set
            {
                if (Readonly) return;
                _fireTime = value;
            }
        }
    }
}